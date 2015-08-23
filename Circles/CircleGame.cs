#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using C3.XNA;

#endregion

namespace Circles {
    public class CircleGame : Game {
        public static CircleGame instance;
        public static int CurrentTurn = Constants.FIRST_PLAYER;

        private InputManager InputManager;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Circle[,] firstPlayerField, secondPlayerField;

        private Circle[,] CurrentField
        {
            get {
                return CurrentTurn == 0 ? firstPlayerField : secondPlayerField;
            }
        }

        private Line currentLine;

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
            this.InputManager.OnMouseDown += OnMouseDown;
            this.InputManager.OnClick += OnMouseUp;

            InitFields();
        }

        public void InitFields() {
            firstPlayerField = new Circle[Constants.FIELD_WIDTH, Constants.FIELD_HEIGHT];
            secondPlayerField = new Circle[Constants.FIELD_WIDTH, Constants.FIELD_HEIGHT];

            for (int i = 0; i < Constants.FIELD_WIDTH; i++) {
                for (int j = 0; j < Constants.FIELD_HEIGHT; j++) {
                    firstPlayerField[i, j] = new Circle(i, j, Constants.FIRST_PLAYER);
                    secondPlayerField[i, j] = new Circle(i, j, Constants.SECOND_PLAYER);
                }
            }
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime) {
            base.Update(gameTime);

            InputManager.Update();
            UpdateLines();

            for (int i = 0; i < Constants.FIELD_WIDTH; i++) {
                for (int j = 0; j < Constants.FIELD_HEIGHT; j++) {
                    firstPlayerField[i, j].Update(gameTime);
                    secondPlayerField[i, j].Update(gameTime);
                }
            }
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
                if (InField(end)) {

                    Vector2 size = begin - end;
                    if (size.LengthSquared() >= 0.99 && size.LengthSquared() <= 1.01) {
                        Console.WriteLine("Added line");
                    }
                }

                currentLine = null;
            }
        }

        private void UpdateLines() {
            if (currentLine != null) {
                currentLine.end = InputManager.GetMousePosition();
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
            DrawField();
            if (currentLine != null) {
                currentLine.Draw(spriteBatch);
            }
            spriteBatch.End();
        }

        protected void DrawField() {

            for (int i = 0; i < Constants.FIELD_WIDTH; i++) {
                for (int j = 0; j < Constants.FIELD_HEIGHT; j++) {
                    firstPlayerField[i, j].Draw(spriteBatch);
                    secondPlayerField[i, j].Draw(spriteBatch);
                }
            }
        }
    }
}
