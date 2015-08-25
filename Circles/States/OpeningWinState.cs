using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using C3.XNA;

namespace Circles.States {
    public class OpeningWinState : State {
        private int turn;
        private float animationTime;
        private CircleGame game;
        private float lineWidth;
        private float imagesHeight = 0f;

        public OpeningWinState(int turn) {
            this.turn = turn;
            this.game = CircleGame.instance;
            this.animationTime = 0f;
            this.lineWidth = 0f;
        }

        public void Update(GameTime gameTime) {
            animationTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (animationTime >= Constants.OPEN_WIN_SCREEN_ANIMATION_TIME) {
                //CircleGame.CurrentState = new WinState(turn);
                animationTime = Constants.OPEN_WIN_SCREEN_ANIMATION_TIME;
            }

            if (animationTime <= Constants.LINE_ANIMATION_TIME) {
                lineWidth = Constants.Animate(animationTime, 0, 0.5f, Constants.LINE_ANIMATION_TIME);
            } else {
                lineWidth = 0.5f;

                float time = animationTime - Constants.LINE_ANIMATION_TIME;
                float doneWhen = Constants.OPEN_WIN_SCREEN_ANIMATION_TIME - Constants.LINE_ANIMATION_TIME;

                imagesHeight = Constants.Animate(time, 0, 1, doneWhen);
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            Color color = Constants.COLORS[turn];

            Vector2 a = Constants.ToScreen(0.5f - lineWidth / 2, 0.5f);
            Vector2 b = Constants.ToScreen(0.5f + lineWidth / 2, 0.5f);
            float lineThikness = Constants.ToScreenMin(Constants.LINE_THICKNESS) / 5f;
            Primitives2D.DrawLine(spriteBatch, a, b, color, lineThikness);
            
            // Draw won label
            Texture2D wonTexture = turn == Constants.FIRST_PLAYER ? CircleGame.FirstWon : CircleGame.SecondWon;
            
            float width = Constants.ToScreenWidth(lineWidth) * 0.9f;
            float height = (width * wonTexture.Height) / wonTexture.Width;
            Rectangle destRect = new Rectangle((int)(Constants.ToScreenWidth(0.5f) - width / 2),
                                               (int)(a.Y - height * imagesHeight - lineThikness), 
                                               (int)width, (int)(height * imagesHeight));
            Rectangle sourceRect = new Rectangle(0, 0, wonTexture.Width, (int)(wonTexture.Height * imagesHeight));
            spriteBatch.Draw(wonTexture, destRect, sourceRect, color);

            // Draw replay label
            Texture2D replay = CircleGame.Replay;
            float replayWidth = (height * replay.Width) / replay.Height;
            Rectangle replayDestRect = new Rectangle((int)(Constants.ToScreenWidth(0.5f) - replayWidth / 2),
                                                     (int)(a.Y + lineThikness * 2),
                                                     (int)replayWidth, (int)(height * imagesHeight));
            Rectangle replaySourceRect = new Rectangle(0, replay.Height - (int)(replay.Height * imagesHeight), 
                                                       replay.Width, (int)(replay.Height * imagesHeight));
            spriteBatch.Draw(replay, replayDestRect, replaySourceRect, color);
        }
    }
}
