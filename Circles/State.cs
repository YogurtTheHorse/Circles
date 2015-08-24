using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Circles {
    public interface State {
        void Update(GameTime gameTime);

        void Draw(SpriteBatch batch);
    }
}
