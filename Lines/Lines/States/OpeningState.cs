using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Lines.Utils;

namespace Lines.States {
    public class OpeningState : IState {
        private LinesGame game;
        private float animationTime;
        private bool isLocal;

        public OpeningState(bool isLocal) {
            this.isLocal = isLocal;
            this.game = LinesGame.instance;
            this.animationTime = 0;

            this.game.InitFields();
        }

        public void Update(GameTime gameTime) {
            animationTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            game.FirstPlayerField.OpenAnimation(gameTime);
            game.SecondPlayerField.OpenAnimation(gameTime);

            if (animationTime > Constants.OPEN_ANIMATION_TIME) {
                if (isLocal) {
                    LinesGame.CurrentState = new GameState();
                } else {
                    LinesGame.CurrentState = new NetGameState();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            game.DrawField();
        }

        public void OnExit() { }
    }
}
