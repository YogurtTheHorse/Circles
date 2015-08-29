using System;
using Lidgren.Network;

namespace Lines.Network {
    public static class NetworkGlobals {
        private static string AppID = "lines";
        private static int Port = 53900; // Port = 0; for c in 'lines': Port += ord(c); Port *= 100

        public static NetPeerConfiguration GetClientConfig() {
            NetPeerConfiguration config = new NetPeerConfiguration(AppID);
            config.Port = Port;
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);

            return config;
        }
    }
}
