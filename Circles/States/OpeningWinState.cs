using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using C3.XNA;

namespace Circles.States {
    public class PreWinState : State {
        private int turn;
        private bool isOpening;

        private CircleGame game;
        private float animationTime = 0f;
        private float lineWidth = 0f;
        private float imagesHeight = 0f;

        public PreWinState(int turn, bool isOpening) {
            this.turn = turn;
            this.isOpening = isOpening;

            this.game = CircleGame.instance;
        }

        public void Update(GameTime gameTime) {
            animationTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (animationTime >= Constants.OPEN_WIN_SCREEN_ANIMATION_TIME) {
                CircleGame.CurrentState = isOpening ? (new WinState(turn) as State) : (new OpeningState() as State);
            }

            if (isOpening) {
                if (animationTime <= Constants.LINE_ANIMATION_TIME) {
                    lineWidth = Constants.Animate(animationTime, 0, 0.5f, Constants.LINE_ANIMATION_TIME);
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
