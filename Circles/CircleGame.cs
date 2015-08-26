#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using C3.XNA;
using System.Collections.Generic;
using Circles.States;
using System.Threading;

#endregion

namespace Circles {
    public class CircleGame : Game {
        public static CircleGame instance;
        public static bool IsMobile;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public Field FirstPlayerField, SecondPlayerField;

        public static State CurrentState;

        public static Texture2D FirstWon, SecondWon, Replay;

        public CircleGame(bool isMobile) {
            instance = this;
            IsMobile = isMobile;

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize() {
            base.Initialize();

            this.Window.AllowUserResizing = true;
            this.IsMouseVisible = true;
            this.graphics.PreferMultiSampling = true;

            InitFields();
            //CurrentState = new PreWinState(0, true);
            CurrentState = new OpeningState();
        }

        public void InitFields() {
            FirstPlayerField = new Field(Constants.FIRST_PLAYER);
            SecondPlayerField = new Field(Constants.SECOND_PLAYER);
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            FirstWon = Content.Load<Texture2D>("1st-won");
            SecondWon = Content.Load<Texture2D>("2nd-won");
            Replay = Content.Load<Texture2D>("replay");
        }

        protected override void Update(GameTime gameTime) {
            base.Update(gameTime);

            CurrentState.Update(gameTime);
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
