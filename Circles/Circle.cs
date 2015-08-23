#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using C3.XNA;

#endregion

namespace Circles {
    public class Circle {
        private static Texture2D texture = CreateTexture(300);

        private Vector2 position;
        private Color color;
        private int player;

        public Circle(int i, int j, int player) {
            this.position = new Vector2(i, j);
            this.player = player;
            this.color = Constants.COLORS[player];
        }

        public void Draw(SpriteBatch batch) {
            float fieldSize = Constants.ToScreenMin(1 - Constants.FIELD_OFFSET * 2);

            Vector2 offset = new Vector2
            {
                X = (CircleGame.instance.GetScreenWidth() - fieldSize) / 2,
                Y = (CircleGame.instance.GetScreenHeight() - fieldSize) / 2
            };

            Vector2 step = new Vector2(fieldSize / (Constants.FIELD_WIDTH - 1));
            if (player == Constants.FIRST_PLAYER) {
                offset -= step / 4;
            } else {
                offset += step / 4;
            }

            Vector2 center = offset + position * step;
            float r = Constants.ToScreenMin(Constants.CIRCLE_RADIUS);

            Vector2 scale = new Vector2(r / texture.Width);
            Vector2 origin = new Vector2(texture.Width / 2);

            batch.Draw(texture, center, null, null, origin, 0, scale, color);
        }

        static Texture2D CreateTexture(int radius) {
            Texture2D texture = new Texture2D(CircleGame.instance.GraphicsDevice, radius, radius);
            Color[] colorData = new Color[radius * radius];

            float diam = radius / 2f;
            float diamsq = diam * diam;

            for (int x = 0; x < radius; x++) {
                for (int y = 0; y < radius; y++) {
                    int index = x * radius + y;
                    Vector2 pos = new Vector2(x - diam, y - diam);
                    if (pos.LengthSquared() <= diamsq) {
                        colorData[index] = Color.White;
                    } else {
                        colorData[index] = Color.Transparent;
                    }
                }
            }

            texture.SetData(colorData);
            return texture;
        }
    }
}
