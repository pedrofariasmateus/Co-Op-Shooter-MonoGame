using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame2DShooterPrototype.Source.Core
{
    public class Enemy
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public bool IsActive { get; set; } = true;
        public int Health { get; set; } = 1;

        public Enemy(Texture2D texture, Vector2 position, Vector2 velocity)
        {
            Texture = texture;
            Position = position;
            Velocity = velocity;
        }

        public void Update(GameTime gameTime)
        {
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive)
                spriteBatch.Draw(Texture, Position, Color.Red);
        }

        public Rectangle GetBounds()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }
    }
}
