using System;
using Lidgren.Network;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using Lines.Utils;
using Microsoft.Xna.Framework;

namespace Lines.Network {
    public class Lobbie {
        private long CreatedAt;

        private bool GameStarted;

        public Field FirstPlayerField, SecondPlayerField;
        public int CurrentTurn;

        public Field CurrentField { get { return CurrentTurn == 0 ? FirstPlayerField : SecondPlayerField; } }
        public Field NextField { get { return CurrentTurn == 1 ? FirstPlayerField : SecondPlayerField; } }

        public bool LobbieWorking;

        private List<NetConnection> connections;

        private Action<string> log;
        private NetServer server;

        public Lobbie(Action<string> log, NetServer server) {
            this.log = log;
            this.server = server;

            CreatedAt = Environment.TickCount;
            connections = new List<NetConnection>();
            
            GameStarted = false;
            LobbieWorking = true;
        }

        public void Log(string s) {
            log(GetHash() + ": " + s);
        }

        public bool IsFull() {
            if (connections.Count == 2) {
                foreach(NetConnection con in connections) {
                    if (con.Status != NetConnectionStatus.Connected) {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        public string GetHash() {
            byte[] encodedPassword = new UTF8Encoding().GetBytes(CreatedAt.ToString());
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);

            return BitConverter.ToString(hash).Replace("-", string.Empty).Substring(0, 6).ToLower();
        }

        public void Connect(NetConnection conn) {
            conn.Tag = this;

            connections.Add(conn);

            log(conn.RemoteEndPoint + " connected.");
        }

        public void Update() {
            if (IsFull() && !GameStarted) {
                StartGame();
            }
        }

        private void StartGame() {
            if (!GameStarted) {
                GameStarted = true;

                Log("Game started");
                for (int i = 0; i < connections.Count; i++) {
                    Log("Sending message about game start to " + connections[i].RemoteEndPoint + "...");

                    NetOutgoingMessage msg = server.CreateMessage();
                    msg.Write((byte)EventType.GameStarted);
                    msg.Write(i);

                    server.SendMessage(msg, connections[i], NetDeliveryMethod.ReliableOrdered);
                }

                Constants.RandomColorScheme();
                FirstPlayerField = new Field(Constants.FIRST_PLAYER);
                SecondPlayerField = new Field(Constants.SECOND_PLAYER);

                CurrentTurn = 0;
            }
        }

        public void PlayerDisconnect() {
            Log("Someone disconnected. Stoping game");
            NetOutgoingMessage msg = server.CreateMessage();

            msg.Write((byte)EventType.Disconnected);
            msg.Write("Another player disconnected");

            SendToAll(msg);

            foreach (NetConnection conn in connections) {
                conn.Disconnect("bg");
            }

            LobbieWorking = false;
        }

        public void WorkWithData(NetIncomingMessage inc) {
            int ind = inc.ReadInt32();
            EventType e = (EventType)inc.ReadByte();
            NetOutgoingMessage msg = server.CreateMessage();

            switch (e) {
                case EventType.ConnectCircles:
                    Vector2 begin = inc.ReadVector2();
                    Vector2 end = inc.ReadVector2();

                    Log(ind + " player tring to connect " + begin + " w/ " + end);

                    if (ind == CurrentTurn && Connect(begin, end)) {
                        Log("He did it");
                        msg.Write((byte)EventType.ConnectCircles);
                        msg.Write(CurrentTurn);
                        msg.Write(begin);
                        msg.Write(end);

                        SendToAll(msg);

                        if (Circle.CheckWon(CurrentField, CurrentTurn)) {
                            SendWon();
                        }

                        NextTurn();
                    } else {
                        Log("Lol no");
                        msg.Write((byte)EventType.RemoveLine);
                        Line.Write(msg, Line.ReadLine(inc, true), true);

                        SendToAll(msg);
                    }
                    break;

                case EventType.CurrentLine:
                    Line l = Line.ReadLine(inc, true);

                    msg.Write((byte)EventType.CurrentLine);
                    Line.Write(msg, l, true);
                    SendToAll(msg, inc.SenderConnection);
                    break;
            }
        }

        private void SendToAll(NetOutgoingMessage msg, NetConnection ignore=null) {
            server.SendMessage(msg, connections, NetDeliveryMethod.ReliableOrdered, 0);
        }

        private void SendWon() {
            Log(CurrentTurn + " won. Sad but true");
            NetOutgoingMessage msg = server.CreateMessage();

            msg.Write((byte)EventType.OnWon);
            msg.Write(CurrentTurn);

            SendToAll(msg);
        }

        private bool Connect(Vector2 begin, Vector2 end) {
            return NextField.Allows(begin, end) && CurrentField.Connect(begin, end);
        }

        private void NextTurn() {
            CurrentTurn = (++CurrentTurn) % 2;

            Log(CurrentTurn + " turn.");

            NetOutgoingMessage msg = server.CreateMessage();
            msg.Write((byte)EventType.NextTurn);
            msg.Write(CurrentTurn);

            SendToAll(msg);
        }

        private bool CanMove() {
            return CurrentField.CanMove(NextField);
        }
    }
}