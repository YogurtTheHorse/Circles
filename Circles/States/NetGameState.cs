using Lines.Network;

namespace Lines.States {
    public class NetGameState : GameState {
        private LinesClient client;

        public NetGameState() : base() {
            client = new LinesClient(this);
        }
    }
}
