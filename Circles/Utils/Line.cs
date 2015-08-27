using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using C3.XNA;

using Lines.States;

namespace Lines.Utils {
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

            this.color = Constants.COLORS[GameState.CurrentTurn];
            this.thickness = Constants.LINE_THICKNESS;
        }

        public Line(Point begin, Point end) {
            this.begin = new Vector2(begin.X, begin.Y);
            this.end = new Vector2(end.X, end.Y);

            this.color = Constants.COLORS[GameState.CurrentTurn];
            this.thickness = Constants.LINE_THICKNESS;
        }

        public bool Remove(GameTime gameTime) {
            if (animationEnd == Vector2.Zero) {
                animationEnd = end;
            }

            removeAnimationTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            end = begin + (animationEnd - begin) * (1 - Constants.Animate(removeAnimationTime, 0, 1, Constants.LINE_ANIMATION_TIME));

            return removeAnimationTime >= Constants.LINE_ANIMATION_TIME;
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