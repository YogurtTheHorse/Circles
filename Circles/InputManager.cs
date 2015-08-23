#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using C3.XNA;

#endregion

namespace Circles {
    public class InputManager {
        public enum MouseButton {
            Left = 0,
            Right
        }

        public delegate void ClickContainer(MouseButton button,Point position);

        public event ClickContainer OnClick;
        public event ClickContainer OnMouseDown;

        public bool IsMouseDown { get; private set; }

        private MouseState OldState;

        public InputManager() {
            this.OldState = new MouseState();
            this.IsMouseDown = false;
        }

        public void Update() {
            MouseState newState = Mouse.GetState();

            if (IsDown(newState) && !IsDown(OldState)) {
                if (OnMouseDown != null) {
                    OnMouseDown(GetButton(newState), newState.Position);
                }
                IsMouseDown = true;
            }

            if (!IsDown(newState) && IsMouseDown) {
                if (OnClick != null) {
                    OnClick(GetButton(OldState), newState.Position);
                }
                IsMouseDown = false;
            }

            OldState = newState;
        }

        private bool IsDown(MouseState state) {
            return state.LeftButton == ButtonState.Pressed || state.RightButton == ButtonState.Pressed;
        }

        private MouseButton GetButton(MouseState state) {
            if (state.LeftButton == ButtonState.Pressed) {
                return MouseButton.Left;
            } else {
                return MouseButton.Right;
            }
        }

        public Vector2 GetMousePosition() {
            return new Vector2(OldState.Position.X, OldState.Position.Y);
        }

    }
}