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

        public static Texture2D FirstWon, SecondWon, DrawWom, Replay;
        public static Texture2D[] WonTextures;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public static SpriteFont Font, BigFont;

        private int currentSong = 0;
        private Song[] songs;

        public Field FirstPlayerField, SecondPlayerField;

        public static State CurrentState;

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

            Color color = Constants.COLORS[Constants.DRAW];
            Texture2D first = StringToTexture("  Lines  ", BigFont);
            Texture2D second = StringToTexture("");

            PreSelectState.OnSelectHandler onChoose = delegate () {
                LinesGame.CurrentState = new OpeningState();
            };

            CurrentState = new PreSelectState(color, first, second, onChoose, true);
        }

        public void InitFields() {
            FirstPlayerField = new Field(Constants.FIRST_PLAYER);
            SecondPlayerField = new Field(Constants.SECOND_PLAYER);
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Font = Content.Load<SpriteFont>("Roboto");
            BigFont = Content.Load<SpriteFont>("Roboto-Big");

            FirstWon = StringToTexture("1st palyer won!");
            SecondWon = StringToTexture("2nd palyer won!");
            DrawWom = StringToTexture("Seems like it's draw");

            WonTextures = new Texture2D[] { FirstWon, SecondWon, DrawWom };

            Replay = StringToTexture("Replay?");

            /*songs = new Song[] {
                Song.FromUri("song17", new Uri("Content/song17.wav", UriKind.Relative)),
                Song.FromUri("song18", new Uri("Content/song18.wav", UriKind.Relative))

            };*/
            
            //MediaPlayer.Play(songs[currentSong]);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.MediaStateChanged += delegate (object sender, EventArgs args) {
                MediaPlayer.Play(songs[(++currentSong) % songs.Length]);
            };
        }

        private Texture2D StringToTexture(string label) {
            return StringToTexture(label, Font);
        }

        private Texture2D StringToTexture(string label, SpriteFont font) {
            Vector2 size = font.MeasureString(label);
            RenderTarget2D renderTarget = new RenderTarget2D(GraphicsDevice, (int)size.X, (int)size.Y);

            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin();
            spriteBatch.DrawString(font, label, Vector2.Zero, Color.White);
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
