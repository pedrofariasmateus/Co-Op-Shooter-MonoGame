using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame2DShooterPrototype.Source.Core;

namespace MonoGame2DShooterPrototype.Source.Entities
{
    public class Player : IEntity
    {
        public Vector2 Position;
        public float Speed = 200f;
        public Texture2D Texture;
        public PlayerInputSettings InputSettings;
        public bool IsActive { get; set; } = true;

        private int screenWidth;
        private int screenHeight;

        public Player(Texture2D texture, Vector2 startPosition, PlayerInputSettings inputSettings, int screenWidth = 800, int screenHeight = 600)
        {
            Texture = texture;
            Position = startPosition;
            InputSettings = inputSettings;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
        }

        public void Update(GameTime gameTime, int viewportWidth, int viewportHeight)
        {
            Vector2 direction = Vector2.Zero;
            KeyboardState keyboardState = Keyboard.GetState();

            if (InputSettings.KeyboardEnabled)
            {
                if (keyboardState.IsKeyDown(GetKey(InputSettings.KeyboardKeybindings.MoveUp)))
                    direction.Y -= 1;
                if (keyboardState.IsKeyDown(GetKey(InputSettings.KeyboardKeybindings.MoveDown)))
                    direction.Y += 1;
                if (keyboardState.IsKeyDown(GetKey(InputSettings.KeyboardKeybindings.MoveLeft)))
                    direction.X -= 1;
                if (keyboardState.IsKeyDown(GetKey(InputSettings.KeyboardKeybindings.MoveRight)))
                    direction.X += 1;
            }
            // Controller support can be added here

            if (direction != Vector2.Zero)
                direction.Normalize();

            Position += direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Clamp position to screen bounds (top-left corner, use float math for accuracy)
            float maxX = viewportWidth - Texture.Width;
            float maxY = viewportHeight - Texture.Height;
            Position.X = MathHelper.Clamp(Position.X, 0f, maxX);
            Position.Y = MathHelper.Clamp(Position.Y, 0f, maxY);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw at clamped position
            spriteBatch.Draw(Texture, Position, Color.White);
        }

        private Keys GetKey(string keyName)
        {
            // Map string to Keys enum
            return keyName switch
            {
                "W" => Keys.W,
                "A" => Keys.A,
                "S" => Keys.S,
                "D" => Keys.D,
                "Up" => Keys.Up,
                "Down" => Keys.Down,
                "Left" => Keys.Left,
                "Right" => Keys.Right,
                "Space" => Keys.Space,
                "Enter" => Keys.Enter,
                _ => Keys.None
            };
        }
    }
}
