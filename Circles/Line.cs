using Microsoft.Xna.Framework;

namespace Circles {
    public struct Line {
        public Vector2 begin;
        public Vector2 end;

        public Line(Vector2 begin, Vector2 end) {
            this.begin = begin;
            this.end = end;
        }
    }
}