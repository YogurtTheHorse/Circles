#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using C3.XNA;
using Microsoft.Xna.Framework.Input.Touch;

#endregion

namespace Circles {
    public class InputManager {
        public enum MouseButton {
            Left = 0,
            Right
        }

        public delegate void ClickContainer(MouseButton button,Vector2 position);

        public event ClickContainer OnClick;
        public event ClickContainer OnMouseDown;

        public bool IsMouseDown { get; private set; }

        private MouseState OldState;
        private TouchCollection OldTouchState;

        public InputManager() {
            this.OldState = new MouseState();
            this.OldTouchState = new TouchCollection();
            this.IsMouseDown = false;
        }

        public void Update() {
            MouseState newState = Mouse.GetState();
            TouchCollection newTouchState = TouchPanel.GetState();

            if ((IsDown(newState) && !IsDown(OldState)) || (newTouchState.Count > 0 && OldTouchState.Count == 0)) {
                if (OnMouseDown != null) {
                    OnMouseDown(GetButton(newState), GetMousePosition());
                }
                IsMouseDown = true;
            }

            if ((!IsDown(newState) || newTouchState.Count == 0) && IsMouseDown) {
                if (OnClick != null) {
                    OnClick(GetButton(OldState), GetMousePosition());
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
            if (OldTouchState.Count > 0) {
                return OldTouchState[0].Position;
            } else {
                return new Vector2(OldState.Position.X, OldState.Position.Y);
            }
        }

    }
}