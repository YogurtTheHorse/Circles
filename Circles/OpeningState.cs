using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Circles {
    public class OpeningState : State {
        private CircleGame game;

        public OpeningState() {
            this.game = CircleGame.instance;
        }

        public void Update(GameTime gameTime) {
            for (int i = 0; i < Constants.FIELD_WIDTH; i++) {
                for (int j = 0; j < Constants.FIELD_HEIGHT; j++) {
                    game.FirstPlayerField[i, j].Animate(gameTime);
                    game.SecondPlayerField[i, j].Animate(gameTime);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            game.DrawField();
        }
    }
}
