#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using C3.XNA;
using System.Collections.Generic;

#endregion

namespace Circles {
    public class CircleGame : Game {
        public static CircleGame instance;
        public static int CurrentTurn = Constants.FIRST_PLAYER;

        private InputManager InputManager;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public Field FirstPlayerField, SecondPlayerField;

        public Field CurrentField { get { return CurrentTurn == 0 ? FirstPlayerField : SecondPlayerField; } }

        public Field NextField { get { return CurrentTurn == 1 ? FirstPlayerField : SecondPlayerField; } }

        private Line currentLine;
        private List<Line> OldLines;

        public static State CurrentState;

        public CircleGame() {
            instance = this;

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize() {
            base.Initialize();

            this.Window.AllowUserResizing = true;
            this.IsMouseVisible = true;
            this.graphics.PreferMultiSampling = true;

            this.InputManager = new InputManager();
            /*this.InputManager.OnMouseDown += OnMouseDown;
            this.InputManager.OnClick += OnMouseUp;*/

            this.OldLines = new List<Line>();

            CurrentState = new OpeningState();

            InitFields();
        }

        public void InitFields() {
            FirstPlayerField = new Field(Constants.FIRST_PLAYER);
            SecondPlayerField = new Field(Constants.SECOND_PLAYER);
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime) {
            base.Update(gameTime);

            CurrentState.Update(gameTime);

            /*InputManager.Update();
            UpdateLines(gameTime);

            for (int i = 0; i < Constants.FIELD_WIDTH; i++) {
                for (int j = 0; j < Constants.FIELD_HEIGHT; j++) {
                    firstPlayerField[i, j].Update(gameTime);
                    secondPlayerField[i, j].Update(gameTime);
                }
            }*/
        }

        // Calls in update if mouse just up
        public void OnMouseDown(InputManager.MouseButton button, Vector2 position) {
            Vector2 circlePosition = Circle.GetPosition(position, CurrentTurn);
            if (InField(circlePosition)) {
                Circle c = CurrentField[(int)circlePosition.X, (int)circlePosition.Y];

                currentLine = new Line(Circle.GetCenterPosition(c), position);
            }
        }

        public void OnMouseUp(InputManager.MouseButton button, Vector2 position) {
            if (currentLine != null) {
                Vector2 begin = Circle.GetPosition(currentLine.begin, CurrentTurn);
                Vector2 end = Circle.GetPosition(currentLine.end, CurrentTurn);
                if (InField(end) && Connect(begin, end)) {
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
            return NextField.Allows(begin, end) && CurrentField.Connect(begin, end);
        }

        private void NextTurn() {
            CurrentTurn = (++CurrentTurn) % 2;
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

        public bool InField(Vector2 v) {
            return new Rectangle(0, 0, Constants.FIELD_WIDTH, Constants.FIELD_HEIGHT).Contains(v);
        }

        public Vector2 GetScreenSize() {
            return new Vector2
            {
                X = GetScreenWidth(),
                Y = GetScreenHeight()
            };
        }

        public float GetScreenWidth() {
            return GraphicsDevice.Viewport.Width;
        }

        public float GetScreenHeight() {
            return GraphicsDevice.Viewport.Height;
        }

        protected override void Draw(GameTime gameTime) {
            graphics.GraphicsDevice.Clear(Color.White);

            base.Draw(gameTime);
            spriteBatch.Begin();

            CurrentState.Draw(spriteBatch);

            /*if (currentLine != null) {
                currentLine.Draw(spriteBatch);
            }

            foreach (Line l in OldLines) {
                l.Draw(spriteBatch);
            }*/
            spriteBatch.End();
        }

        public void DrawField() {
            FirstPlayerField.Draw(spriteBatch);
            SecondPlayerField.Draw(spriteBatch);
        }
    }
}
