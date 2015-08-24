using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using C3.XNA;

namespace Circles {
    public class Line {
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
        }

        public void Draw(SpriteBatch batch) {
            Primitives2D.DrawLine(batch, begin, end, color, Constants.ToScreenMax(thickness));
        }
    }
}