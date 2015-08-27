using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Lines.States {
    public interface State {
        void Update(GameTime gameTime);

        void Draw(SpriteBatch batch);
    }
}
