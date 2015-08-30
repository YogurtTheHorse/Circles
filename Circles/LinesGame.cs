using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Lines.States;
using Lines.Utils;
using Microsoft.Xna.Framework.Media;
using System;

namespace Lines {
    public class LinesGame : Game {
        public static LinesGame instance;
        public static bool IsMobile;

        public static Texture2D GameTitle, LocalGame, MultiplayerGame;
        public static Texture2D FirstWon, SecondWon, DrawWom, Replay, ToMainMenu;
        public static Texture2D[] WonTextures;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public static SpriteFont Font, BigFont;

        private MusicManager MusicManager;

        public Field FirstPlayerField, SecondPlayerField;

        public static State CurrentState;

        public static Network.LinesClient client;

        public LinesGame(bool isMobile) {
            instance = this;
            IsMobile = isMobile;

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize() {
            base.Initialize();

            this.Window.Title = "Lines";
            this.Window.AllowUserResizing = true;
            this.IsMouseVisible = true;
            this.graphics.PreferMultiSampling = true;
            this.graphics.IsFullScreen = IsMobile;

            Constants.RandomColorScheme();
        }

        public void InitFields() {
            Constants.RandomColorScheme();

            FirstPlayerField = new Field(Constants.FIRST_PLAYER);
            SecondPlayerField = new Field(Constants.SECOND_PLAYER);
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Font = Content.Load<SpriteFont>("Roboto");
            BigFont = Content.Load<SpriteFont>("Roboto-Big");

            GameTitle = StringToTexture("  Lines  ", BigFont);
            LocalGame = StringToTexture("        Local game        ");
            MultiplayerGame = StringToTexture("LAN Multiplayer (Beta)");

            FirstWon = StringToTexture("1st palyer won!");
            SecondWon = StringToTexture("2nd palyer won!");
            DrawWom = StringToTexture("Seems like it's draw");

            WonTextures = new Texture2D[] { FirstWon, SecondWon, DrawWom };

            Replay = StringToTexture("Replay?");
            ToMainMenu = StringToTexture("Go to main menu");

            MusicManager = new MusicManager();
        }

        protected override void BeginRun() {
            base.BeginRun();

            CurrentState = new PreMainMenu(true);
        }

        public static Texture2D StringToTexture(string label) {
            return StringToTexture(label, Font);
        }

        public static Texture2D StringToTexture(string label, SpriteFont font) {
            GraphicsDevice GraphicsDevice = instance.GraphicsDevice;

            Vector2 size = font.MeasureString(label);
            RenderTarget2D renderTarget = new RenderTarget2D(GraphicsDevice, (int)size.X, (int)size.Y);

            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.Transparent);

            instance.spriteBatch.Begin();
            instance.spriteBatch.DrawString(font, label, Vector2.Zero, Color.White);
            instance.spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            return (Texture2D)renderTarget;
        }

        protected override void Update(GameTime gameTime) {
            base.Update(gameTime);

            MusicManager.Update(gameTime);
        
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
