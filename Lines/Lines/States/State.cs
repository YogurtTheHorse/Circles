using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Lines.States {
    public interface IState {
        void Update(GameTime gameTime);

        void Draw(SpriteBatch batch);

        void OnExit();
    }
}
