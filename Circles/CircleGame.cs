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
            this.InputManager.OnClick += OnMouseDown;

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

            for (int i = 0; i < Constants.FIELD_WIDTH; i++) {
                for (int j = 0; j < Constants.FIELD_HEIGHT; j++) {
                    firstPlayerField[i, j].Update(gameTime);
                    secondPlayerField[i, j].Update(gameTime);
                }
            }
        }

        // Calls in update if mouse just up
        public void OnMouseDown(InputManager.MouseButton button) {
            Console.WriteLine(button.ToString() + " button is down");
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
