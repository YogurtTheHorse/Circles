using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Lines.Utils;

namespace Lines.States {
    public class OpeningState : State {
        private LinesGame game;
        private float animationTime;

        public OpeningState() {
            this.game = LinesGame.instance;
            this.animationTime = 0;

            this.game.InitFields();
        }

        public void Update(GameTime gameTime) {
            animationTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            game.FirstPlayerField.OpenAnimation(gameTime);
            game.SecondPlayerField.OpenAnimation(gameTime);

            if (animationTime > Constants.OPEN_ANIMATION_TIME) {
                LinesGame.CurrentState = new GameState();
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            game.DrawField();
        }
    }
}
