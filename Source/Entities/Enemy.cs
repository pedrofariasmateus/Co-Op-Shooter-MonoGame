using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame2DShooterPrototype.Source.Entities
{
    public class Enemy : IEntity
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

        public void Update(GameTime gameTime, int viewportWidth, int viewportHeight)
        {
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            // Deactivate if off-screen
            if (Position.Y > viewportHeight || Position.X + Texture.Width < 0 || Position.X > viewportWidth)
            {
                IsActive = false;
            }
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
