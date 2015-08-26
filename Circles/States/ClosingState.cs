using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Circles.States {
    public class ClosingState : State {
        private int turn;
        private float animationTime;
        private CircleGame game;

        public ClosingState(int turn) {
            this.turn = turn;
            this.game = CircleGame.instance;
            this.animationTime = 0;

            this.game.FirstPlayerField.ResetAnimation();
            this.game.SecondPlayerField.ResetAnimation();
        }

        public void Update(GameTime gameTime) {
            animationTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (animationTime >= Constants.CLOSE_ANIMATION_TIME) {
                Color color = Constants.COLORS[turn];
                Texture2D wonTexture = CircleGame.WonTextures[turn];

                PreSelectState.OnSelectHandler onChoose = delegate () {
                    CircleGame.CurrentState = new OpeningState();
                };

                CircleGame.CurrentState = new PreSelectState(color, wonTexture, CircleGame.Replay, onChoose, true);
                return;
            }

            float fieldAnimation = (Constants.CLOSE_ANIMATION_TIME - Constants.LINE_ANIMATION_TIME) / 2;

            if (animationTime < Constants.LINE_ANIMATION_TIME + fieldAnimation) {
                game.FirstPlayerField.CloseAnimation(gameTime);
            }
            if (animationTime < Constants.LINE_ANIMATION_TIME || animationTime > Constants.LINE_ANIMATION_TIME + fieldAnimation) {
                game.SecondPlayerField.CloseAnimation(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            game.DrawField();
        }
    }
}
