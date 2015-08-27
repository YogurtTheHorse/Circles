using C3.XNA;

using Lines.Utils;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lines.States {
    public class SelectState : State {
        private LinesGame game;
        private float lineWidth;
        private InputManager inputManager;

        private Color color;
        private Texture2D first;
        private Texture2D second;
        private PreSelectState.OnSelectHandler onFirst;
        private PreSelectState.OnSelectHandler onSecond;

        public SelectState(Color color, Texture2D first, Texture2D second, PreSelectState.OnSelectHandler onFirst, PreSelectState.OnSelectHandler onSecond) {
            this.color = color;
            this.first = first;
            this.second = second;
            this.onFirst = onFirst;
            this.onSecond = onSecond;

            this.game = LinesGame.instance;
            this.lineWidth = 0.5f;

            this.inputManager = new InputManager();
            this.inputManager.OnClick += OnDown;
        }

        public void Update(GameTime gameTime) {
            inputManager.Update();
        }

        private void OnDown(InputManager.MouseButton button, Vector2 position) {
            if (onSecond == null) {
                onSecond = onFirst;
            } else if (onFirst == null) {
                onFirst = onSecond;
            }

            if (position.Y < Constants.ToScreenHeight(0.5f)) {
                LinesGame.CurrentState = new PreSelectState(color, first, second, onFirst, false);
            } else {
                LinesGame.CurrentState = new PreSelectState(color, first, second, onSecond, false);
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            Vector2 a = Constants.ToScreen(0.5f - lineWidth / 2, 0.5f);
            Vector2 b = Constants.ToScreen(0.5f + lineWidth / 2, 0.5f);
            float lineThikness = Constants.ToScreenMin(Constants.LINE_THICKNESS) / 5f;
            Primitives2D.DrawLine(spriteBatch, a, b, color, lineThikness);

            // Draw won label
            float width = Constants.ToScreenWidth(lineWidth) * 0.9f;
            float height = (width * first.Height) / first.Width;
            Rectangle destRect = new Rectangle((int)(Constants.ToScreenWidth(0.5f) - width / 2),
                                               (int)(a.Y - height - lineThikness),
                                               (int)width, (int)height);
            spriteBatch.Draw(first, destRect, null, color);

            // Draw replay label
            float replayWidth = (height * second.Width) / second.Height;
            Rectangle replayDestRect = new Rectangle((int)(Constants.ToScreenWidth(0.5f) - replayWidth / 2),
                                                     (int)(a.Y + lineThikness * 2),
                                                     (int)replayWidth, (int)height);
            spriteBatch.Draw(second, replayDestRect, null, color);
        }
    }
}
