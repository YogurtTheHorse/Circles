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
                //CircleGame.CurrentState = new WinAnimation    State(turn);
                return;
            }

            game.FirstPlayerField.CloseAnimation(gameTime);
            game.SecondPlayerField.CloseAnimation(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch) {
            game.DrawField();
        }
    }
}
