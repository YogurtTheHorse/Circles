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

        private bool IsMobile;
        public PlatformID s;

        public InputManager() {
            this.OldState = new MouseState();
            this.OldTouchState = new TouchCollection();
            this.IsMouseDown = false;
            
            this.IsMobile = CircleGame.IsMobile;
        }

        public void Update() {
			if (IsMobile) {
				UpdateTouches ();
			} else {
				UpdateMouse ();
			}
        }

		private void UpdateMouse() {
			MouseState newState = Mouse.GetState();

			if (IsDown(newState) && !IsDown(OldState)) {
				if (OnMouseDown != null) {
					OnMouseDown(GetButton(newState), GetMousePosition());
				}
				IsMouseDown = true;
			}

			if (!IsDown(newState) && IsMouseDown) {
				if (OnClick != null) {
					OnClick(GetButton(OldState), GetMousePosition());
				}
				IsMouseDown = false;
			}

			OldState = newState;
        }

		private void UpdateTouches() {
			TouchCollection newTouchState = TouchPanel.GetState();

			if (IsTouching(newTouchState) && !IsTouching(OldTouchState)) {
				if (OnMouseDown != null) {
					OnMouseDown(MouseButton.Left, GetMousePosition());
				}
				IsMouseDown = true;
			}

			if (!IsTouching(newTouchState) && IsMouseDown) {
				if (OnClick != null) {
					OnClick(MouseButton.Left, GetMousePosition());
				}
				IsMouseDown = false;
			}

			OldTouchState = newTouchState;
        }

		bool IsTouching (TouchCollection touchCollection) {
			foreach (TouchLocation tl in touchCollection) {
				if (tl.State == TouchLocationState.Moved || tl.State == TouchLocationState.Pressed) {
					return true;
				}
			}
			return false;
		}

        private bool IsDown(MouseState state) {
            return state.LeftButton == ButtonState.Pressed || state.RightButton ==  ButtonState.Pressed;
        }

        private MouseButton GetButton(MouseState state) {
            if (state.LeftButton == ButtonState.Pressed) {
                return MouseButton.Left;
            } else {
                return MouseButton.Right;
            }
        }

        public Vector2 GetMousePosition() {
			if (IsMobile) {
				foreach (TouchLocation tl in OldTouchState) {
					if (tl.State == TouchLocationState.Moved || tl.State == TouchLocationState.Pressed) {
						return tl.Position;
					} else {
						Console.WriteLine (tl.State);
					}
				}
				return Vector2.Zero;
            } else {
                return new Vector2(OldState.Position.X, OldState.Position.Y);
            }
        }

    }
}