using Lidgren.Network;
using Lines.Network;
using Lines.Utils;
using Microsoft.Xna.Framework;
using System;

namespace Lines.Network {
    public class LinesServer {
        private NetPeerConfiguration config;
        private NetServer server;
        private bool serverWorking;

        private Action<string> log;

        private bool GameStarted;
        
        public Field FirstPlayerField, SecondPlayerField;
        public int CurrentTurn;

        public Field CurrentField { get { return CurrentTurn == 0 ? FirstPlayerField : SecondPlayerField; } }
        public Field NextField { get { return CurrentTurn == 1 ? FirstPlayerField : SecondPlayerField; } }

        public LinesServer() {
            config = NetworkGlobals.GetConfig();
            config.Port = NetworkGlobals.Port;

            server = new NetServer(config);
        }

        public void Start(Action<string> log) {
            this.log = log;

            server.Start();
            serverWorking = true;

            GameStarted = false;

            log("Server started at 0.0.0.0:" + NetworkGlobals.Port);
            while (serverWorking) {
                Update();
            }
        }

        private void Update() {
            NetIncomingMessage inc;
            while ((inc = server.ReadMessage()) != null) {
                switch (inc.MessageType) {
                    case NetIncomingMessageType.DiscoveryRequest:
                        log("Discovery request from " + inc.SenderEndPoint);

                        NetOutgoingMessage response = server.CreateMessage();
                        server.SendDiscoveryResponse(response, inc.SenderEndPoint);
                        break;

                    case NetIncomingMessageType.ConnectionApproval:
                        NetConnection conn = inc.SenderConnection;

                        log("Someone trying to connect from " + inc.SenderEndPoint);
                        if (server.ConnectionsCount < 2) {
                            log("Lucky one. Connected");
                            log("Now we have " + (server.ConnectionsCount + 1) + " players");

                            conn.Approve();
                        } else {
                            conn.Disconnect("Server full");
                            log("Disconnected - Server full");
                        }
                        break;

                    case NetIncomingMessageType.Data:
                        WorkWithData(inc);
                        break;
                }
            }

            if (server.ConnectionsCount == 2 && !GameStarted) {
                StartGame();
            }
        }

        private void WorkWithData(NetIncomingMessage inc) {
            int ind = inc.ReadInt32();
            EventType e = (EventType)inc.ReadByte();
            NetOutgoingMessage msg = server.CreateMessage();

            switch (e) {
                case EventType.ConnectCircles:
                    Vector2 begin = inc.ReadVector2();
                    Vector2 end = inc.ReadVector2();

                    log(ind + " player tring to connect " + begin + " w/ " + end);

                    if (ind == CurrentTurn && Connect(begin, end)) {
                        log("He did it");
                        msg.Write((byte)EventType.ConnectCircles);
                        msg.Write(CurrentTurn);
                        msg.Write(begin);
                        msg.Write(end);

                        server.SendToAll(msg, NetDeliveryMethod.ReliableOrdered);

                        NextTurn();
                    } else {
                        log("Lol no");
                        msg.Write((byte)EventType.RemoveLine);
                        msg.Write(CurrentTurn);
                        msg.Write(begin);
                        msg.Write(end);

                        server.SendToAll(msg, NetDeliveryMethod.ReliableOrdered);
                    }
                    break;

                case EventType.CurrentLine:
                    Line l = Line.ReadLine(inc, true);

                    msg.Write((byte)EventType.CurrentLine);
                    Line.Write(msg, l, true);
                    server.SendToAll(msg, inc.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0);
                    break;
            }
        }

        private void StartGame() {
            if (!GameStarted) {
                GameStarted = true;

                log("Game started");
                for (int i = 0; i < server.ConnectionsCount; i++) {
                    log("Sending message about game start to " + server.Connections[i].RemoteEndPoint + "...");

                    NetOutgoingMessage msg = server.CreateMessage();
                    msg.Write((byte)EventType.GameStarted);
                    msg.Write(i);

                    server.SendMessage(msg, server.Connections[i], NetDeliveryMethod.ReliableOrdered);
                }

                Constants.RandomColorScheme();
                FirstPlayerField = new Field(Constants.FIRST_PLAYER);
                SecondPlayerField = new Field(Constants.SECOND_PLAYER);

                CurrentTurn = 0;
            }
        }

        private bool Connect(Vector2 begin, Vector2 end) {
            return NextField.Allows(begin, end) && CurrentField.Connect(begin, end);
        }

        private void NextTurn() {
            CurrentTurn = (++CurrentTurn) % 2;

            NetOutgoingMessage msg = server.CreateMessage();
            msg.Write((byte)EventType.NextTurn);
            msg.Write(CurrentTurn);

            server.SendToAll(msg, NetDeliveryMethod.ReliableOrdered);
        }

        private bool CanMove() {
            return CurrentField.CanMove(NextField);
        }
    }
}
