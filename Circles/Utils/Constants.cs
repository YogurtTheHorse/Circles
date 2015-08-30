using System;
using Microsoft.Xna.Framework;

namespace Lines.Utils {
    public class Constants {
        public const int FIELD_SIZE = 5;
        public const int FIELD_WIDTH = FIELD_SIZE;
        public const int FIELD_HEIGHT = FIELD_SIZE;
        public const float FIELD_OFFSET = 0.15f;

        public const float CIRCLE_RADIUS = 0.04f;

        public const int FIRST_PLAYER = 0;
        public const int SECOND_PLAYER = 1;
        public const int DRAW = 2;
        public const int NOONE = DRAW;
        public static Color[] COLORS;

        public const float CHANGE_STATUS_ANIMATION_TIME = 1f;
        public const float OPEN_ANIMATION_TIME = 1f;
        public const float CLOSE_ANIMATION_LAST_CIRCLE_START = (CLOSE_ANIMATION_TIME - LINE_ANIMATION_TIME) / 10f;
        public const float CLOSE_ANIMATION_TIME = LINE_ANIMATION_TIME + 2f;
        public const float OPEN_WIN_SCREEN_ANIMATION_TIME = LINE_ANIMATION_TIME + 0.3f;
        public const float LINE_ANIMATION_TIME = 0.5f;
        public const float LINE_THICKNESS = 0.015f;

        public static void RandomColorScheme() {
            Color[][] colorScemes = new Color[][] {
                new Color[] {
                    new Color(235, 73, 73),
                    new Color(99, 200, 225),
                    Color.Black
                },
                new Color[] {
                    new Color(190, 128, 190),
                    new Color(91, 98, 190),
                    Color.Black
                },
                new Color[] {
                    new Color(235, 172, 95),
                    new Color(48, 190, 56),
                    Color.Black
                }
            };

            Random rand = new Random();
            COLORS = colorScemes[rand.Next(colorScemes.Length)];
        }

        public static Vector2 ToField(Vector2 v) {
            return Circle.ToField(v);
        }

        public static Vector2 FromField(Vector2 v) {
            return Circle.FromField(v);
        }

        public static Vector2 ToScreen(float x, float y) {
            return new Vector2(ToScreenWidth(x), ToScreenHeight(y));
        }

        public static Vector2 ToScreen(Vector2 world) {
            return ToScreen(world.X, world.Y);
        }

        public static float ToScreenWidth(float w) {
            return w * LinesGame.instance.GetScreenWidth();
        }

        public static float ToScreenHeight(float h) {
            return h * LinesGame.instance.GetScreenHeight();
        }

        public static float ToScreenMin(float x) {
            return Math.Min(ToScreenWidth(x), ToScreenHeight(x));
        }

        public static float ToScreenMax(float x) {
            return Math.Max(ToScreenWidth(x), ToScreenHeight(x));
        }

        public static float Animate(float t, float b, float c, float d) {
            return c * (t /= d) * t * (2.70158f * t - 1.70158f) + b;
        }

        public static float CloseAnimate(float t, float b, float c, float d) {
            return c * (t /= d) * t * (3 * 1.70158f * t - 1.70158f) + b;
        }

        public static float SelectScreenAnimate(float t, float b, float c, float d) {
            return c * ((t = t / d - 1) * t * ((1.70158f + 1) * t + 1.70158f) + 1) + b;
        }
    }
}
