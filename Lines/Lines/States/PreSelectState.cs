using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using C3.XNA;

using Lines.Utils;
using System;

namespace Lines.States {
    public class PreSelectState : IState {
        public delegate void OnSelectHandler();
        
        protected bool isOpening;

        protected LinesGame game;
        protected float animationTime = 0f;
        protected float lineWidth = 0f;
        protected float imagesHeight = 0f;

        protected Color color;
        protected Texture2D title;
        protected Texture2D first;
        protected Texture2D second;
        protected OnSelectHandler onFirst;
        protected OnSelectHandler onSecond;

        public PreSelectState(Color color, Texture2D title, Texture2D first, Texture2D second, OnSelectHandler onFirst, OnSelectHandler onSecond, bool isOpening) {
            this.color = color;
            this.title = title;
            this.first = first;
            this.second = second;
            this.onFirst = onFirst;
            this.onSecond = onSecond;
            this.isOpening = isOpening;

            this.game = LinesGame.instance;
        }

        public PreSelectState(Color color, Texture2D title, Texture2D first, Texture2D second, OnSelectHandler onFirst, bool isOpening) :
            this(color, title, first, second, onFirst, null, isOpening) { }

        public void Update(GameTime gameTime) {
            animationTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (animationTime >= Constants.OPEN_WIN_SCREEN_ANIMATION_TIME) {
                if (isOpening) {
                    LinesGame.CurrentState = new SelectState(color, title, first, second, onFirst, onSecond);
                } else {
                    onFirst();
                }
            }

            if (isOpening) {
                if (animationTime <= Constants.LINE_ANIMATION_TIME) {
                    lineWidth = Constants.SelectScreenAnimate(animationTime, 0, 0.5f, Constants.LINE_ANIMATION_TIME);
                } else {
                    lineWidth = 0.5f;

                    float time = animationTime - Constants.LINE_ANIMATION_TIME;
                    float doneWhen = Constants.OPEN_WIN_SCREEN_ANIMATION_TIME - Constants.LINE_ANIMATION_TIME;

                    imagesHeight = Constants.Animate(time, 0, 1, doneWhen);
                }
            } else {
                float l = Constants.OPEN_WIN_SCREEN_ANIMATION_TIME - Constants.LINE_ANIMATION_TIME;
                if (animationTime > l) {
                    lineWidth = 0.5f - Constants.Animate(animationTime - l, 0, 0.5f, Constants.LINE_ANIMATION_TIME);
                    imagesHeight = 0;
                } else {
                    lineWidth = 0.5f;
                    
                    float doneWhen = Constants.OPEN_WIN_SCREEN_ANIMATION_TIME - Constants.LINE_ANIMATION_TIME;

                    imagesHeight = 1 - Constants.Animate(animationTime, 0, 1, doneWhen);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            DrawTitle(spriteBatch);
            DrawButtons(spriteBatch);
        }

        private void DrawTitle(SpriteBatch spriteBatch) {
            Vector2 pos = Constants.ToScreen(0.5f, -1 + 6f / 5f * imagesHeight);
            float scale = Constants.ToScreenWidth(0.4f) / title.Width;

            spriteBatch.Draw(title, pos, null, color, 0f, new Vector2(title.Width / 2, title.Height / 2), scale, SpriteEffects.None, 0);
        }

        private void DrawButtons(SpriteBatch spriteBatch) {
            Vector2 a = Constants.ToScreen(0.5f - lineWidth / 2, 0.5f);
            Vector2 b = Constants.ToScreen(0.5f + lineWidth / 2, 0.5f);
            float lineThikness = Constants.ToScreenMin(Constants.LINE_THICKNESS) / 5f;
            Primitives2D.DrawLine(spriteBatch, a, b, color, lineThikness);

            // Draw won label            
            float width = Constants.ToScreenWidth(lineWidth) * 0.9f;
            float height = (width * first.Height) / first.Width;
            Rectangle destRect = new Rectangle((int)(Constants.ToScreenWidth(0.5f) - width / 2),
                                               (int)(a.Y - height * imagesHeight - lineThikness),
                                               (int)width, (int)(height * imagesHeight));
            Rectangle sourceRect = new Rectangle(0, 0, first.Width, (int)(first.Height * imagesHeight));
            spriteBatch.Draw(first, destRect, sourceRect, color);

            // Draw replay label
            float replayWidth = (height * second.Width) / second.Height;
            Rectangle replayDestRect = new Rectangle((int)(Constants.ToScreenWidth(0.5f) - replayWidth / 2),
                                                     (int)(a.Y + lineThikness * 2),
                                                     (int)replayWidth, (int)(height * imagesHeight));
            Rectangle replaySourceRect = new Rectangle(0, second.Height - (int)(second.Height * imagesHeight),
                                                       second.Width, (int)(second.Height * imagesHeight));
            spriteBatch.Draw(second, replayDestRect, replaySourceRect, color);
        }

        public void OnExit() { }
    }
}
