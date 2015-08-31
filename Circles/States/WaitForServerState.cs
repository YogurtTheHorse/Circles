
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Lines.Utils;

namespace Lines.States {
    public class WaitForServerState : IState {
        private string realStatus;
        private string currentStatus;

        private float animationScale, animationTime;

        private bool gameStarted;

        public WaitForServerState(bool isLANGame) {
            LinesGame.client = new Network.LinesClient(isLANGame);
            LinesGame.client.OnGameStarted += GameStarted;
            LinesGame.client.OnConnect += OnConnect;

            this.realStatus = this.currentStatus = "Looking for server...";
            this.animationScale = 0f;
            this.animationTime = 0f;
        }

        private void OnConnect() {
            realStatus = "Waiting for second player...";
            animationTime = Constants.OPEN_WIN_SCREEN_ANIMATION_TIME - 0.001f;
        }

        private void GameStarted(int yourIndex) {
            gameStarted = true;
            animationTime = 0f;
        }

        public void Draw(SpriteBatch batch) {
            Vector2 size = LinesGame.Font.MeasureString(currentStatus);
            float scale = Constants.ToScreenWidth(0.5f) / size.X;
            Vector2 scaledSize = size * scale;

            Vector2 position = Constants.ToScreen(0.5f, 0f) + new Vector2(0f, scaledSize.Y * animationScale);
            position -= new Vector2(scaledSize.X / 2, scaledSize.Y);
            Color color = Constants.COLORS[Constants.DRAW];

            batch.DrawString(LinesGame.Font, currentStatus, position, color, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }

        public void Update(GameTime gameTime) {
            LinesGame.client.Update(gameTime);

            if (animationTime < 0) {
                if (currentStatus != realStatus) {
                    currentStatus = realStatus;
                    animationTime = 0f;
                } else if (gameStarted) {
                    LinesGame.CurrentState = new OpeningState(false);
                }
            } else if (animationTime < Constants.CHANGE_STATUS_ANIMATION_TIME) {
                if (currentStatus == realStatus) {
                    if (gameStarted) {
                        animationTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    } else {
                        animationTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                } else {
                    animationTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

                animationScale = Constants.Animate(animationTime, 0, 1, Constants.CHANGE_STATUS_ANIMATION_TIME);
            }
        }

        public void OnExit() {
            LinesGame.client.Disconnect();
        }
    }
}
