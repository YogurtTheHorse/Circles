using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using C3.XNA;

using Lines.States;
using System;
using Lidgren.Network;
using Lines.Utils;
using Lines.Network;

namespace Lines.Utils {
    public class Line {
        private float removeAnimationTime = 0;
        private Vector2 animationEnd;

        private int turn;

        private bool wasGrowing;

        public Vector2 begin;
        public Vector2 end;
        public Color color;
        public float thickness;

        public Line(Vector2 begin, Vector2 end) {
            this.begin = begin;
            this.end = end;

            this.wasGrowing = false;

            this.color = Constants.COLORS[GameState.CurrentTurn];
            this.thickness = Constants.LINE_THICKNESS;

            this.turn = GameState.CurrentTurn;
        }

        public Line(Point begin, Point end) : 
            this(new Vector2(begin.X, begin.Y), new Vector2(end.X, end.Y)) { }

        public static void Write(NetOutgoingMessage message, Line l, bool isServer=false) {
            message.Write(l.removeAnimationTime);
            if (isServer) {
                message.Write(l.animationEnd);
            } else {
                message.Write(Constants.ToField(l.animationEnd));
            }
            message.Write(l.turn);
            message.Write(l.wasGrowing);
            if (isServer) {
                message.Write(l.begin);
                message.Write(l.end);
            } else {
                message.Write(Constants.ToField(l.begin));
                message.Write(Constants.ToField(l.end));
            }
            message.Write(l.thickness);
        }

        public static Line ReadLine(NetIncomingMessage message, bool isServer=false) {
            float removeAnimation = message.ReadFloat();
            Vector2 animationEnd = message.ReadVector2();
            int turn = message.ReadInt32(); 
            bool wasGrowing = message.ReadBoolean();
            Vector2 begin = message.ReadVector2();
            Vector2 end = message.ReadVector2();
            float thickness = message.ReadFloat();

            if (!isServer) {
                animationEnd = Constants.FromField(animationEnd);
                begin = Constants.FromField(begin);
                end = Constants.FromField(end);
            }

            Line l = new Line (begin, end);
            l.removeAnimationTime = removeAnimation;
            l.animationEnd = animationEnd;
            l.turn = turn;
            l.color = Constants.COLORS[turn];
            l.wasGrowing = wasGrowing;
            l.thickness = thickness;

            return l;
        }

        public bool Remove(GameTime gameTime) {
            if (animationEnd == Vector2.Zero) {
                animationEnd = end;
            }

            if (wasGrowing) {
                wasGrowing = false;
                removeAnimationTime = Constants.LINE_ANIMATION_TIME - removeAnimationTime;
            }

            return Animate(false, gameTime);
        }

        public bool Grow(GameTime gameTime) {
            wasGrowing = true;

            return Animate(true, gameTime);
        }

        private bool Animate(bool grow, GameTime gameTime) {
            if (removeAnimationTime >= Constants.LINE_ANIMATION_TIME) {
                return true;
            }

            removeAnimationTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (grow) {
                end = begin + (animationEnd - begin) * Constants.Animate(removeAnimationTime, 0, 1, Constants.LINE_ANIMATION_TIME);
            } else {
                end = begin + (animationEnd - begin) * (1 - Constants.Animate(removeAnimationTime, 0, 1, Constants.LINE_ANIMATION_TIME));
            }

            return false;
        }

        public void SetAnimationPosition(Vector2 pos) {
            animationEnd = end;
            end = pos;
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