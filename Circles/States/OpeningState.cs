using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Circles.States {
    public class OpeningState : State {
        private CircleGame game;
        private float animationTime;

        public OpeningState() {
            this.game = CircleGame.instance;
            this.animationTime = 0;
        }

        public void Update(GameTime gameTime) {
            animationTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            for (int i = 0; i < Constants.FIELD_WIDTH; i++) {
                for (int j = 0; j < Constants.FIELD_HEIGHT; j++) {
                    game.FirstPlayerField[i, j].Animate(gameTime);
                    game.SecondPlayerField[i, j].Animate(gameTime);
                }
            }

            if (animationTime >= Constants.OPEN_ANIMATION_TIME) {
                CircleGame.CurrentState = new GameState();
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            game.DrawField();
        }
    }
}
