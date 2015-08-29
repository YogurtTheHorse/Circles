using Lidgren.Network;
using Lines.States;

namespace Lines.Network {
    public class LinesClient {
        private NetPeerConfiguration config;
        private NetClient client;

        private NetGameState gameState;

        public LinesClient(NetGameState gameState) {
            this.gameState = gameState;

            config = NetworkGlobals.GetClientConfig();
            client = new NetClient(config);

            client.Start();
        }

        public void Update() {
            NetIncomingMessage msg;
            while ((msg = client.ReadMessage()) != null) {
                switch (msg.MessageType) {
                    case NetIncomingMessageType.DiscoveryResponse:
                        // just connect to first server discovered
                        client.Connect(msg.SenderEndPoint);
                        break;
                }
            }
        }
    }
}
