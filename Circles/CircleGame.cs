#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

#endregion

namespace Circles {
	public class Game1 : Game {
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		public Game1() {
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";	            
			graphics.IsFullScreen = true;		
		}

		protected override void Initialize() {
			base.Initialize();
		}

		protected override void LoadContent() {
			spriteBatch = new SpriteBatch(GraphicsDevice);
		}

		protected override void Update(GameTime gameTime) {
			#if !__IOS__
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
			    Keyboard.GetState().IsKeyDown(Keys.Escape)) {
				Exit();
			}
			#endif
			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime) {
			graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            
			base.Draw(gameTime);
		}
	}
}
