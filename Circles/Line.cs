using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using C3.XNA;

namespace Circles {
    public class Line {
        public Vector2 begin;
        public Vector2 end;

        public Line(Vector2 begin, Vector2 end) {
            this.begin = begin;
            this.end = end;
        }

        public Line(Point begin, Point end) {
            this.begin = new Vector2(begin.X, begin.Y);
            this.end = new Vector2(end.X, end.Y);
        }

        public void Draw(SpriteBatch batch) {
            Primitives2D.DrawLine(batch, begin, end, Constants.COLORS[CircleGame.CurrentTurn], 10f);
        }
    }
}