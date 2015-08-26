using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;

namespace Circles.States {
    public class GameState : State {
        public static GameState instance;

        private CircleGame game;

        private InputManager InputManager;

        private Line currentLine;
        private List<Line> OldLines;

        public static int CurrentTurn;

        public Field CurrentField { get { return CurrentTurn == 0 ? game.FirstPlayerField : game.SecondPlayerField; } }

        public Field NextField { get { return CurrentTurn == 1 ? game.FirstPlayerField : game.SecondPlayerField; } }

        public GameState() {
            GameState.instance = this;
            CurrentTurn = Constants.FIRST_PLAYER;

            this.game = CircleGame.instance;

            this.InputManager = new InputManager();
            this.InputManager.OnMouseDown += OnMouseDown;
            this.InputManager.OnClick += OnMouseUp;

            this.OldLines = new List<Line>();
        }

        // Calls in update if mouse just up
        public void OnMouseDown(InputManager.MouseButton button, Vector2 position) {
            Vector2 circlePosition = Circle.GetPosition(position, CurrentTurn);
            if (CurrentField.InField(circlePosition)) {
                Circle c = CurrentField[(int)circlePosition.X, (int)circlePosition.Y];

                currentLine = new Line(Circle.GetCenterPosition(c), position);
            }
        }

        public void OnMouseUp(InputManager.MouseButton button, Vector2 position) {
            if (currentLine != null) {
                Vector2 begin = Circle.GetPosition(currentLine.begin, CurrentTurn);
                Vector2 end = Circle.GetPosition(currentLine.end, CurrentTurn);
                if (CurrentField.InField(end) && Connect(begin, end)) {
                    NextTurn();
                } else {
                    OldLines.Add(currentLine);
                }

                currentLine = null;
            }
        }

        // Returns true if connection was succesful
        private bool Connect(Vector2 begin, Vector2 end) {
            return NextField.Allows(begin, end) && CurrentField.Connect(begin, end);
        }

        private void NextTurn() {
            CurrentTurn = (++CurrentTurn) % 2;

            if (!CanMove()) {
                CircleGame.CurrentState = new ClosingState(Constants.DRAW);
            }
        }

        private bool CanMove() {
            return CurrentField.CanMove(NextField);
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
            game.DrawField();

            if (currentLine != null) {
                currentLine.Draw(spriteBatch);
            }

            foreach (Line l in OldLines) {
                l.Draw(spriteBatch);
            }
        }
    }

}
