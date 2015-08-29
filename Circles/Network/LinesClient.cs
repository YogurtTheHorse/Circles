using System;
using Lidgren.Network;
using Lines.States;
using Microsoft.Xna.Framework;

namespace Lines.Network {
    public class LinesClient {
        public delegate void OnConnectHandler();
        public delegate void OnGameStartedHandler(int yourIndex);
        public delegate void OnConnectCirclesHandler(int turn, Vector2 begin, Vector2 end);
        public delegate void OnNextTurnHandler(int turn);

        public OnConnectHandler OnConnect;
        public OnGameStartedHandler OnGameStarted;
        public OnConnectCirclesHandler OnConnectCircles;
        public OnNextTurnHandler OnNextTurn;

        private NetPeerConfiguration config;
        private NetClient client;

        private NetConnection connection;

        public int PlayerIndex = 2;

        public LinesClient() {
            config = NetworkGlobals.GetConfig();
            client = new NetClient(config);

            client.Start();
            client.DiscoverLocalPeers(NetworkGlobals.Port);
        }

        public void Update() {
            NetIncomingMessage msg;
            while ((msg = client.ReadMessage()) != null) {
                switch (msg.MessageType) {
                    case NetIncomingMessageType.DiscoveryResponse:
                        connection = client.Connect(msg.SenderEndPoint);
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        NetConnectionStatus status = (NetConnectionStatus)msg.ReadByte();
                        switch (status) {
                            case NetConnectionStatus.Disconnected:
                                connection = null;
                                break;

                            case NetConnectionStatus.Connected:
                                if (OnConnect != null) {
                                    OnConnect();
                                }
                                break;
                        }
                        break;

                    case NetIncomingMessageType.Data:
                        WorkWithData(msg);
                        break;
                }
                client.Recycle(msg);
            }
        }

        public void ConnectCircles(Vector2 begin, Vector2 end) {
            NetOutgoingMessage msg = CreateMessage();
            msg.Write((byte)EventType.ConnectCircles);
            msg.Write(begin);
            msg.Write(end);
            
            client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
        }

        private NetOutgoingMessage CreateMessage() {
            NetOutgoingMessage msg = client.CreateMessage();
            msg.Write(PlayerIndex);

            return msg;
        }

        private void WorkWithData(NetIncomingMessage msg) {
            EventType e = (EventType)msg.ReadByte();

            switch (e) {
                case EventType.GameStarted:
                    int i = msg.ReadInt32();

                    PlayerIndex = i;

                    if (OnGameStarted != null) {
                        OnGameStarted(i);
                    }
                    break;

                case EventType.ConnectCircles:
                    if (OnConnectCircles != null) {
                        OnConnectCircles(msg.ReadInt32(), msg.ReadVector2(), msg.ReadVector2());
                    }
                    break;

                case EventType.NextTurn:
                    if (OnNextTurn != null) {
                        OnNextTurn(msg.ReadInt32());
                    }
                    break;
            }
        }
    }
}
