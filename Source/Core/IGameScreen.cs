using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.UI;

namespace MonoGame2DShooterPrototype.Source.Core
{
    public interface IGameScreen
    {
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
        Desktop GetMyraDesktop();
    }
}
