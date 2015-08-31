using System;
using System.Net;
using Lidgren.Network;

namespace Lines.Network {
    public static class NetworkGlobals {
        private const string AppID = "lines";
        public const string IP = "37.252.127.227";
        public const int Port = 53900; // Port = 0; for c in 'lines': Port += ord(c); Port *= 100

        public static NetPeerConfiguration GetConfig() {
            NetPeerConfiguration config = new NetPeerConfiguration(AppID);
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);

            return config;
        }
    }
}
