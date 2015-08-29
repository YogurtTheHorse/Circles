
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Lines.Utils;

namespace Lines.States {
    public class WaitForServerState : State {
        public WaitForServerState() {
            LinesGame.client = new Network.LinesClient();
            LinesGame.client.OnGameStarted += (i) => { LinesGame.CurrentState = new OpeningState(false); };
        }

        public void Draw(SpriteBatch batch) {
            string text = "Waiting for some server...";
            Vector2 position = Constants.ToScreen(0.5f, 0.5f) - LinesGame.Font.MeasureString(text) / 2;
            Color color = Constants.COLORS[Constants.DRAW];

            batch.DrawString(LinesGame.Font, text, position, color);
        }

        public void Update(GameTime gameTime) {
            LinesGame.client.Update();
        }
    }
}
