using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Lines.Utils;

namespace Lines.States {
    public class ClosingState : State {
        private int turn;
        private float animationTime;
        private LinesGame game;

        public ClosingState(int turn) {
            this.turn = turn;
            this.game = LinesGame.instance;
            this.animationTime = 0;

            this.game.FirstPlayerField.ResetAnimation();
            this.game.SecondPlayerField.ResetAnimation();
        }

        public void Update(GameTime gameTime) {
            animationTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (animationTime >= Constants.CLOSE_ANIMATION_TIME) {
                Color color = Constants.COLORS[turn];
                Texture2D wonTexture = LinesGame.WonTextures[turn];

                PreSelectState.OnSelectHandler onChoose = delegate () {
                    LinesGame.CurrentState = new OpeningState();
                };

                LinesGame.CurrentState = new PreSelectState(color, wonTexture, LinesGame.ToMainMenu, LinesGame.Replay, onChoose, true);
                return;
            }

            float fieldAnimation = (Constants.CLOSE_ANIMATION_TIME - Constants.LINE_ANIMATION_TIME) / 2;

            if (animationTime < Constants.LINE_ANIMATION_TIME + fieldAnimation) {
                game.FirstPlayerField.CloseAnimation(gameTime);
                if (animationTime > Constants.LINE_ANIMATION_TIME) {
                    game.SecondPlayerField.RemoveConnections();
                }
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
