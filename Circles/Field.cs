using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Circles {
    public class Field {
        private Circle[,] circles;
        private int player;

        private List<Line> connections;

        public Field(int player) {
            this.player = player;
            this.circles = new Circle[Constants.FIELD_WIDTH, Constants.FIELD_HEIGHT];

            this.connections = new List<Line>();

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

            if (a.Connect(b)) {
                connections.Add(new Line(Circle.GetCenterPosition(a), Circle.GetCenterPosition(b)));
                return true;
            }

            return false;
        }

        public void Draw(SpriteBatch spriteBatch) {
            foreach (Line l in connections) {
                l.Draw(spriteBatch);
            }

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
