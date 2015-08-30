using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Lines.Utils;

namespace Lines.States {
    public class ClosingState : State {
        private int turn;
        private float animationTime;
        private LinesGame game;
        private bool isNetGame, amWon;

        public ClosingState(int turn) {
            this.turn = turn;
            this.game = LinesGame.instance;
            this.animationTime = 0;

            this.game.FirstPlayerField.ResetAnimation();
            this.game.SecondPlayerField.ResetAnimation();
        }

        public ClosingState(int turn, bool isNetGame, bool amWon) : this(turn) {
            this.isNetGame = isNetGame;
            this.amWon = amWon;
        }

        public void Update(GameTime gameTime) {
            animationTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (animationTime >= Constants.CLOSE_ANIMATION_TIME) {
                if (isNetGame) {Color color = Constants.COLORS[turn];
                    Texture2D first = LinesGame.StringToTexture(amWon ? "You won" : "You lose", LinesGame.BigFont);
                    Texture2D second = LinesGame.StringToTexture("Go to main menu");
                    Texture2D third = LinesGame.StringToTexture("");

                    PreSelectState.OnSelectHandler onChoose = delegate () {
                        LinesGame.CurrentState = new PreMainMenu(true);
                    };

                    LinesGame.CurrentState = new PreSelectState(color, first, second, third, onChoose, true);
                } else {
                    Color color = Constants.COLORS[turn];
                    Texture2D wonTexture = LinesGame.WonTextures[turn];

                    PreSelectState.OnSelectHandler onChoose = delegate () {
                        LinesGame.CurrentState = new OpeningState(false);
                    };

                    LinesGame.CurrentState = new PreSelectState(color, wonTexture, LinesGame.ToMainMenu, LinesGame.Replay, onChoose, true);
                }
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
