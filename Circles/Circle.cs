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

        private float animationTime;
        private float radius;

        public Circle(int i, int j, int player) {
            this.position = new Vector2(i, j);
            this.player = player;
            this.color = Constants.COLORS[player];

            this.animationTime = 0f;
            this.radius = 0f;
        }

        public void Update(GameTime gameTime) {
            Animate(gameTime);
        }

        private void Animate(GameTime gameTime) {
            if (animationTime <= Constants.OPEN_ANIMATION_TIME) {
                animationTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                float scale = (float)Math.Sin(Math.PI / 4f * 3f * animationTime / Constants.OPEN_ANIMATION_TIME) * 1.5f;
                radius = Constants.CIRCLE_RADIUS * scale;
            } else {
                radius = Constants.CIRCLE_RADIUS;
            }
        }

        public void Draw(SpriteBatch batch) {
            Vector2 center = GetCenterPosition(this);
            float r = Constants.ToScreenMin(radius);

            Vector2 scale = new Vector2(r / texture.Width);
            Vector2 origin = new Vector2(texture.Width / 2);

            batch.Draw(texture, center, null, null, origin, 0, scale, color);
        }

        public static Vector2 GetPosition(Vector2 screenPoint, int player) {
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

            screenPoint -= offset - step / 2;
            screenPoint /= step;

            return new Vector2((int)screenPoint.X, (int)screenPoint.Y);
        }

        public static Vector2 GetCenterPosition(Circle circle) {
            float fieldSize = Constants.ToScreenMin(1 - Constants.FIELD_OFFSET * 2);

            Vector2 offset = new Vector2
            {
                X = (CircleGame.instance.GetScreenWidth() - fieldSize) / 2,
                Y = (CircleGame.instance.GetScreenHeight() - fieldSize) / 2
            };

            Vector2 step = new Vector2(fieldSize / (Constants.FIELD_WIDTH - 1));
            if (circle.player == Constants.FIRST_PLAYER) {
                offset -= step / 4;
            } else {
                offset += step / 4;
            }

            return offset + circle.position * step;
        }

        private static Texture2D CreateTexture(int radius) {
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
