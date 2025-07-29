using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame2DShooterPrototype.Source.Core
{
    public class Bullet
    {
        public Vector2 Position;
        public Vector2 Direction;
        public float Speed = 400f;
        public Texture2D Texture;
        public bool IsActive = true;
        private int screenWidth;
        private int screenHeight;

        public Bullet(Texture2D texture, Vector2 position, Vector2 direction, int screenWidth, int screenHeight)
        {
            Texture = texture;
            Position = position;
            Direction = direction;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
        }

        public void Update(GameTime gameTime)
        {
            Position += Direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            // Deactivate if out of bounds
            if (Position.X < -Texture.Width || Position.X > screenWidth || Position.Y < -Texture.Height || Position.Y > screenHeight)
                IsActive = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive)
                spriteBatch.Draw(Texture, Position, Color.Yellow);
        }

        public Rectangle GetBounds()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }
    }
}
