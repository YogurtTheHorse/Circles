using C3.XNA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Circles.States {
    public class WinState : State {
        private int turn;
        private CircleGame game;
        private float lineWidth;
        private InputManager inputManager;

        public WinState(int turn) {
            this.turn = turn;
            this.game = CircleGame.instance;
            this.lineWidth = 0.5f;

            this.inputManager = new InputManager();
            this.inputManager.OnClick += OnDown;
        }

        public void Update(GameTime gameTime) {
            inputManager.Update();
        }

        private void OnDown(InputManager.MouseButton button, Vector2 position) {
            CircleGame.CurrentState = new PreWinState(turn, false);
        }

        public void Draw(SpriteBatch spriteBatch) {
            Color color = Constants.COLORS[turn];

            Vector2 a = Constants.ToScreen(0.5f - lineWidth / 2, 0.5f);
            Vector2 b = Constants.ToScreen(0.5f + lineWidth / 2, 0.5f);
            float lineThikness = Constants.ToScreenMin(Constants.LINE_THICKNESS) / 5f;
            Primitives2D.DrawLine(spriteBatch, a, b, color, lineThikness);

            // Draw won label
            Texture2D wonTexture = turn == Constants.FIRST_PLAYER ? CircleGame.FirstWon : CircleGame.SecondWon;

            float width = Constants.ToScreenWidth(lineWidth) * 0.9f;
            float height = (width * wonTexture.Height) / wonTexture.Width;
            Rectangle destRect = new Rectangle((int)(Constants.ToScreenWidth(0.5f) - width / 2),
                                               (int)(a.Y - height - lineThikness),
                                               (int)width, (int)height);
            spriteBatch.Draw(wonTexture, destRect, null, color);

            // Draw replay label
            Texture2D replay = CircleGame.Replay;
            float replayWidth = (height * replay.Width) / replay.Height;
            Rectangle replayDestRect = new Rectangle((int)(Constants.ToScreenWidth(0.5f) - replayWidth / 2),
                                                     (int)(a.Y + lineThikness * 2),
                                                     (int)replayWidth, (int)height);
            spriteBatch.Draw(replay, replayDestRect, null, color);
        }
    }
}
