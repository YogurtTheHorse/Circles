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

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public Field FirstPlayerField, SecondPlayerField;

        public Field CurrentField { get { return CurrentTurn == 0 ? FirstPlayerField : SecondPlayerField; } }

        public Field NextField { get { return CurrentTurn == 1 ? FirstPlayerField : SecondPlayerField; } }

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
            spriteBatch.End();
        }

        public void DrawField() {
            FirstPlayerField.Draw(spriteBatch);
            SecondPlayerField.Draw(spriteBatch);
        }
    }
}
