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
        public static SpriteFont Font;
        public static Texture2D FirstWon, SecondWon, DrawWom, Replay;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public Field FirstPlayerField, SecondPlayerField;

        public static State CurrentState;

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
            
            CurrentState = new OpeningState();
        }

        public void InitFields() {
            FirstPlayerField = new Field(Constants.FIRST_PLAYER);
            SecondPlayerField = new Field(Constants.SECOND_PLAYER);
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Font = Content.Load<SpriteFont>("Roboto");

            FirstWon = StringToTexture("1st palyer won!");
            SecondWon = StringToTexture("2nd palyer won!");
            DrawWom = StringToTexture("Seems like it's draw");
            Replay = StringToTexture("Replay?");
        }

        private Texture2D StringToTexture(string label) {
            Vector2 size = Font.MeasureString(label);
            RenderTarget2D renderTarget = new RenderTarget2D(GraphicsDevice, (int)size.X, (int)size.Y);

            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin();
            spriteBatch.DrawString(Font, label, Vector2.Zero, Color.White);
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            return (Texture2D)renderTarget;
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
