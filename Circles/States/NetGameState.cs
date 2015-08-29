using System;
using Lines.Network;
using Microsoft.Xna.Framework;
using Lines.Utils;
using Microsoft.Xna.Framework.Graphics;

namespace Lines.States {
    public class NetGameState : GameState {
        private LinesClient client;

        public NetGameState() : base() {
            client = LinesGame.client;

            client.OnConnectCircles += OnConnectCircles;
            client.OnNextTurn += OnNextTurn;

            client.OnConnectCircles += OnConnectCircles;

            client.OnCurrentLineChanged += OnCurrentLiineChanged;
        }

        // Calls in update if mouse just up
        public override void OnMouseDown(InputManager.MouseButton button, Vector2 position) {
            Vector2 circlePosition = Circle.GetPosition(position, CurrentTurn);
            if (CurrentField.InField(circlePosition) && CurrentTurn == client.PlayerIndex) {
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

        private void OnCurrentLiineChanged(Line newLine) {
            secondPlayerLine = newLine;
        }

        private void OnNextTurn(int turn) {
            CurrentTurn = turn;
        }

        private void OnConnectCircles(int turn, Vector2 begin, Vector2 end) {
            OnNextTurn(turn);
            secondPlayerLine = null;
            CurrentField.ForceConnect(begin, end);
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            client.Update();
            if (client.PlayerIndex == CurrentTurn && currentLine != null) {
                client.SendCurrentLine(currentLine);
            }
        }

        public override void Draw(SpriteBatch spriteBatch) {
            base.Draw(spriteBatch);
            
            if (secondPlayerLine != null) {
                secondPlayerLine.Draw(spriteBatch);
            }
        }
    }
}
