using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Circles {
    public class Field {
        private Circle[,] circles;
        private int player;

        public Field(int player) {
            this.player = player;
            circles = new Circle[Constants.FIELD_WIDTH, Constants.FIELD_HEIGHT];

            for (int i = 0; i < Constants.FIELD_WIDTH; i++) {
                for (int j = 0; j < Constants.FIELD_HEIGHT; j++) {
                    this[i, j] = new Circle(i, j, player);
                }
            }
        }

        public bool Connect(Vector2 begin, Vector2 end) {
            if ((end - begin).LengthSquared() > 1.01 || (end - begin).LengthSquared() < 0.99) {
                return false;
            }

            Circle a = this[begin];
            Circle b = this[end];

            return a.Connect(b);
        }

        public void Draw(SpriteBatch spriteBatch) {
            for (int i = 0; i < Constants.FIELD_WIDTH; i++) {
                for (int j = 0; j < Constants.FIELD_HEIGHT; j++) {
                    circles[i, j].Draw(spriteBatch);
                }
            }
        }

        public Circle this [Vector2 v]
        {
            get { return this[(int)v.X, (int)v.Y]; }
            private set { this[(int)v.X, (int)v.Y] = value; }
        }

        public Circle this [int i, int j]
        {
            get { return circles[i, j]; }
            private set { circles[i, j] = value; }
        }
    }
}
