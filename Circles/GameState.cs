using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Circles {
    public class GameState : State {
        private CircleGame game;

        private InputManager InputManager;

        private Line currentLine;
        private List<Line> OldLines;

        public GameState() {
            this.game = CircleGame.instance;

            this.InputManager = new InputManager();
            this.InputManager.OnMouseDown += OnMouseDown;
            this.InputManager.OnClick += OnMouseUp;

            this.OldLines = new List<Line>();
        }

        // Calls in update if mouse just up
        public void OnMouseDown(InputManager.MouseButton button, Vector2 position) {
            Vector2 circlePosition = Circle.GetPosition(position, CircleGame.CurrentTurn);
            if (game.InField(circlePosition)) {
                Circle c = game.CurrentField[(int)circlePosition.X, (int)circlePosition.Y];

                currentLine = new Line(Circle.GetCenterPosition(c), position);
            }
        }

        public void OnMouseUp(InputManager.MouseButton button, Vector2 position) {
            if (currentLine != null) {
                Vector2 begin = Circle.GetPosition(currentLine.begin, CircleGame.CurrentTurn);
                Vector2 end = Circle.GetPosition(currentLine.end, CircleGame.CurrentTurn);
                if (game.InField(end) && Connect(begin, end)) {
                    Console.WriteLine(Circle.CheckWon());
                    NextTurn();
                } else {
                    OldLines.Add(currentLine);
                }

                currentLine = null;
            }
        }

        // Returns true if connection was succesful
        private bool Connect(Vector2 begin, Vector2 end) {
            return game.NextField.Allows(begin, end) && game.CurrentField.Connect(begin, end);
        }

        private void NextTurn() {
            CircleGame.CurrentTurn = (++CircleGame.CurrentTurn) % 2;
        }

        public void Update(GameTime gameTime) {
            InputManager.Update();
            UpdateLines(gameTime);
        }

        private void UpdateLines(GameTime gameTime) {
            if (currentLine != null) {
                currentLine.end = InputManager.GetMousePosition();
            }

            for (int i = 0; i < OldLines.Count; i++) {
                Line l = OldLines[i];
                if (l.Remove(gameTime)) {
                    OldLines.RemoveAt(i);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            if (currentLine != null) {
                currentLine.Draw(spriteBatch);
            }

            foreach (Line l in OldLines) {
                l.Draw(spriteBatch);
            }

            game.DrawField();
        }
    }

}
