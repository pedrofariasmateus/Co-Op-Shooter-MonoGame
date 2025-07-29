using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame2DShooterPrototype.Source.Entities
{
    public interface IEntity
    {
        void Update(GameTime gameTime, int viewportWidth, int viewportHeight);
        void Draw(SpriteBatch spriteBatch);
    }
}
