using System;
using Lines.Network;
using Microsoft.Xna.Framework;
using Lines.Utils;

namespace Lines.States {
    public class NetGameState : GameState {
        private LinesClient client;

        public NetGameState() : base() {
            client = LinesGame.client;

            client.OnConnectCircles += OnConnectCircles;
            client.OnNextTurn += OnNextTurn;

            client.OnConnectCircles += OnConnectCircles;
        }

        // Calls in update if mouse just up
        public override void OnMouseDown(InputManager.MouseButton button, Vector2 position) {
            Vector2 circlePosition = Circle.GetPosition(position, CurrentTurn);
            if (CurrentField.InField(circlePosition)) {
                Circle c = CurrentField[(int)circlePosition.X, (int)circlePosition.Y];

                currentLine = new Line(Circle.GetCenterPosition(c), position);
            }
        }

        public override void OnMouseUp(InputManager.MouseButton button, Vector2 position) {
            if (currentLine != null) {
                Vector2 begin = Circle.GetPosition(currentLine.begin, CurrentTurn);
                Vector2 end = Circle.GetPosition(currentLine.end, CurrentTurn);
                if (CurrentField.InField(end)) {
                    client.ConnectCircles(begin, end);
                }

                currentLine = null;
            }
        }

        private void OnNextTurn(int turn) {
            CurrentTurn = turn;
        }

        private void OnConnectCircles(int turn, Vector2 begin, Vector2 end) {
            OnNextTurn(turn);
            CurrentField.ForceConnect(begin, end);
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            client.Update();
        }
    }
}
