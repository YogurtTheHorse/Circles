using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using C3.XNA;
using System;

namespace Circles {
    public class Line {
        private float removeAnimationTime = 0;
        private Vector2 animationEnd;

        public Vector2 begin;
        public Vector2 end;
        public Color color;
        public float thickness;

        public Line(Vector2 begin, Vector2 end) {
            this.begin = begin;
            this.end = end;

            this.color = Constants.COLORS[CircleGame.CurrentTurn];
            this.thickness = Constants.LINE_THICKNESS;
        }

        public Line(Point begin, Point end) {
            this.begin = new Vector2(begin.X, begin.Y);
            this.end = new Vector2(end.X, end.Y);

            this.color = Constants.COLORS[CircleGame.CurrentTurn];
            this.thickness = Constants.LINE_THICKNESS;
        }

        public bool Remove(GameTime gameTime) {
            if (animationEnd == Vector2.Zero) {
                animationEnd = end;
            }

            removeAnimationTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            float amount = 1 - removeAnimationTime / Constants.LINE_ANIMATION_TIME;
            end = begin + (animationEnd - begin) * amount;

            return amount < 0;
        }

        public void Draw(SpriteBatch batch) {
            Primitives2D.DrawLine(batch, begin, end, color, Constants.ToScreenMin(thickness));
        }

        public void DrawOnField(SpriteBatch batch, int player) {
            Vector2 a = Circle.GetCenterPosition(begin, player);
            Vector2 b = Circle.GetCenterPosition(end, player);
            Primitives2D.DrawLine(batch, a, b, color, Constants.ToScreenMin(thickness));
        }
    }
}