using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Circles {
    public class Field {
        private Circle[,] circles;
        private int player;

        private int Width;
        private int Height;

        private List<Line> connections;

        public Field(int player) {
            this.Width = Constants.FIELD_WIDTH;
            this.Height = Constants.FIELD_HEIGHT;

            this.player = player;
            this.circles = new Circle[Width, Height];

            this.connections = new List<Line>();

            for (int i = 0; i < Width; i++) {
                for (int j = 0; j < Height; j++) {
                    this[i, j] = new Circle(i, j, player);
                }
            }
        }

        public void OpenAnimation(GameTime gameTime) {
            for (int i = 0; i < Width; i++) {
                for (int j = 0; j < Height; j++) {
                    this[i, j].OpenAnimation(gameTime);
                }
            }
        }

        private static void Swap<T>(ref T lhs, ref T rhs) {
            T temp = lhs;
            lhs = rhs;
            rhs = temp;
        }

        // Returns true if nothing restricts connection of @begin and @end
        // Don't try to understand this
        public bool Allows(Vector2 begin, Vector2 end) {
            Point a = new Point((int)begin.X, (int)begin.Y);
            Point b = new Point((int)end.X, (int)end.Y);

            if (a.Y == b.Y) { // Horizontal
                if (a.X > b.X) {
                    Swap<Point>(ref a, ref b);
                }
                if (player == Constants.FIRST_PLAYER) {
                    return !TryGet(a.X + 1, a.Y + 1).IsConnected(TryGet(a.X + 1, a.Y));
                } else {
                    return !TryGet(a.X, a.Y).IsConnected(TryGet(a.X, a.Y - 1));
                }
            } else { // Vertical
                if (a.Y > b.Y) {
                    Swap<Point>(ref a, ref b);
                }
                if (player == Constants.FIRST_PLAYER) {
                    return !TryGet(a.X + 1, a.Y + 1).IsConnected(TryGet(a.X, a.Y + 1));
                } else {
                    return !TryGet(a.X, a.Y).IsConnected(TryGet(a.X - 1, a.Y));
                }
            }
        }

        private Circle TryGet(int x, int y) {
            if (CircleGame.instance.InField(new Vector2(x, y))) {
                return this[x, y];
            } else {
                return new Circle(0, 0, 0);
            }
        }

        public bool Connect(Vector2 begin, Vector2 end) {
            if ((end - begin).LengthSquared() > 1.01 || (end - begin).LengthSquared() < 0.99) {
                return false;
            }

            Circle a = this[begin];
            Circle b = this[end];

            if (a.Connect(b)) {
                connections.Add(new Line(begin, end));
                return true;
            }

            return false;
        }

        public void Draw(SpriteBatch spriteBatch) {
            foreach (Line l in connections) {
                l.DrawOnField(spriteBatch, player);
            }

            for (int i = 0; i < Width; i++) {
                for (int j = 0; j < Height; j++) {
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
