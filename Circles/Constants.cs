using System;
using Microsoft.Xna.Framework;

namespace Circles {
    public class Constants {
        public const int FIELD_SIZE = 5;
        public const int FIELD_WIDTH = FIELD_SIZE;
        public const int FIELD_HEIGHT = FIELD_SIZE;
        public const float FIELD_OFFSET = 0.1f;

        public const float CIRCLE_RADIUS = 0.05f;

        public static Color FIRST_PLAYER_COLOR = Color.CornflowerBlue;

        public static Vector2 ToScreen(float x, float y) {
            return new Vector2(ToScreenWidth(x), ToScreenHeight(y));
        }

        public static Vector2 ToScreen(Vector2 world) {
            return ToScreen(world.X, world.Y);
        }

        public static float ToScreenWidth(float w) {
            return w * CircleGame.instance.GetScreenWidth();
        }

        public static float ToScreenHeight(float h) {
            return h * CircleGame.instance.GetScreenHeight();
        }

        public static float ToScreenMin(float x) {
            return Math.Min(ToScreenWidth(x), ToScreenHeight(x));
        }

        public static float ToScreenMax(int x) {
            return Math.Max(ToScreenWidth(x), ToScreenHeight(x));
        }
    }
}
