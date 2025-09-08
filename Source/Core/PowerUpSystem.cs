using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MonoGame2DShooterPrototype.Source.Core
{
    public enum PowerUpType
    {
        SpeedBoost,
        RapidFire,
        Shield,
        MultiShot,
        HealthPack
    }

    public class PowerUp
    {
        public PowerUpType Type { get; set; }
        public Vector2 Position { get; set; }
        public bool IsActive { get; set; } = true;
        public float RotationAngle { get; set; }
        public float PulseTimer { get; set; }

        public PowerUp(PowerUpType type, Vector2 position)
        {
            Type = type;
            Position = position;
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            RotationAngle += deltaTime * 2f; // Rotate slowly
            PulseTimer += deltaTime * 3f; // Pulse for attracting attention
        }

        public Rectangle GetBounds()
        {
            return new Rectangle((int)Position.X - 16, (int)Position.Y - 16, 32, 32);
        }
    }

    public static class PowerUpVisuals
    {
        public static Color GetPowerUpColor(PowerUpType type)
        {
            return type switch
            {
                PowerUpType.SpeedBoost => Color.Cyan,
                PowerUpType.RapidFire => Color.Red,
                PowerUpType.Shield => Color.Blue,
                PowerUpType.MultiShot => Color.Purple,
                PowerUpType.HealthPack => Color.Green,
                _ => Color.White
            };
        }

        public static void DrawPowerUp(SpriteBatch spriteBatch, Texture2D pixelTexture, PowerUp powerUp)
        {
            if (!powerUp.IsActive) return;

            var color = GetPowerUpColor(powerUp.Type);
            var position = powerUp.Position;
            
            // Pulsing effect
            float pulse = 0.8f + 0.2f * (float)Math.Sin(powerUp.PulseTimer);
            color *= pulse;
            
            // Main body
            var mainRect = new Rectangle((int)position.X - 12, (int)position.Y - 12, 24, 24);
            spriteBatch.Draw(pixelTexture, mainRect, color);
            
            // Border
            var borderRect = new Rectangle((int)position.X - 14, (int)position.Y - 14, 28, 28);
            DrawBorder(spriteBatch, pixelTexture, borderRect, Color.White * pulse, 2);
            
            // Type-specific visual indicators
            switch (powerUp.Type)
            {
                case PowerUpType.SpeedBoost:
                    // Arrow pointing right
                    var arrow = new Rectangle((int)position.X - 6, (int)position.Y - 2, 12, 4);
                    spriteBatch.Draw(pixelTexture, arrow, Color.White);
                    break;
                    
                case PowerUpType.RapidFire:
                    // Multiple small dots
                    for (int i = 0; i < 3; i++)
                    {
                        var dot = new Rectangle((int)position.X - 6 + i * 6, (int)position.Y - 1, 2, 2);
                        spriteBatch.Draw(pixelTexture, dot, Color.White);
                    }
                    break;
                    
                case PowerUpType.Shield:
                    // Circle outline
                    DrawCircleOutline(spriteBatch, pixelTexture, position, 8, Color.White);
                    break;
                    
                case PowerUpType.MultiShot:
                    // Three lines spreading out
                    var line1 = new Rectangle((int)position.X - 8, (int)position.Y - 1, 16, 2);
                    var line2 = new Rectangle((int)position.X - 6, (int)position.Y - 6, 12, 2);
                    var line3 = new Rectangle((int)position.X - 6, (int)position.Y + 4, 12, 2);
                    spriteBatch.Draw(pixelTexture, line1, Color.White);
                    spriteBatch.Draw(pixelTexture, line2, Color.White);
                    spriteBatch.Draw(pixelTexture, line3, Color.White);
                    break;
                    
                case PowerUpType.HealthPack:
                    // Plus sign
                    var horizontal = new Rectangle((int)position.X - 6, (int)position.Y - 1, 12, 2);
                    var vertical = new Rectangle((int)position.X - 1, (int)position.Y - 6, 2, 12);
                    spriteBatch.Draw(pixelTexture, horizontal, Color.White);
                    spriteBatch.Draw(pixelTexture, vertical, Color.White);
                    break;
            }
        }

        private static void DrawBorder(SpriteBatch spriteBatch, Texture2D pixelTexture, Rectangle bounds, Color color, int thickness)
        {
            // Top
            spriteBatch.Draw(pixelTexture, new Rectangle(bounds.X, bounds.Y, bounds.Width, thickness), color);
            // Bottom  
            spriteBatch.Draw(pixelTexture, new Rectangle(bounds.X, bounds.Y + bounds.Height - thickness, bounds.Width, thickness), color);
            // Left
            spriteBatch.Draw(pixelTexture, new Rectangle(bounds.X, bounds.Y, thickness, bounds.Height), color);
            // Right
            spriteBatch.Draw(pixelTexture, new Rectangle(bounds.X + bounds.Width - thickness, bounds.Y, thickness, bounds.Height), color);
        }

        private static void DrawCircleOutline(SpriteBatch spriteBatch, Texture2D pixelTexture, Vector2 center, int radius, Color color)
        {
            // Simple circle approximation using rectangles
            for (int angle = 0; angle < 360; angle += 15)
            {
                float radians = MathHelper.ToRadians(angle);
                int x = (int)(center.X + Math.Cos(radians) * radius);
                int y = (int)(center.Y + Math.Sin(radians) * radius);
                spriteBatch.Draw(pixelTexture, new Rectangle(x, y, 2, 2), color);
            }
        }
    }

    public class PowerUpManager
    {
        private List<PowerUp> _powerUps;
        private Random _random;
        private float _spawnTimer;
        private const float SpawnInterval = 15f; // Spawn every 15 seconds

        public PowerUpManager()
        {
            _powerUps = new List<PowerUp>();
            _random = new Random();
        }

        public void Update(GameTime gameTime, Rectangle worldBounds)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            // Update existing power-ups
            foreach (var powerUp in _powerUps)
            {
                powerUp.Update(gameTime);
            }

            // Remove inactive power-ups
            _powerUps.RemoveAll(p => !p.IsActive);

            // Spawn new power-ups
            _spawnTimer += deltaTime;
            if (_spawnTimer >= SpawnInterval)
            {
                SpawnRandomPowerUp(worldBounds);
                _spawnTimer = 0f;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D pixelTexture)
        {
            foreach (var powerUp in _powerUps)
            {
                PowerUpVisuals.DrawPowerUp(spriteBatch, pixelTexture, powerUp);
            }
        }

        public PowerUp CheckCollision(Rectangle playerBounds)
        {
            foreach (var powerUp in _powerUps)
            {
                if (powerUp.IsActive && powerUp.GetBounds().Intersects(playerBounds))
                {
                    powerUp.IsActive = false;
                    return powerUp;
                }
            }
            return null;
        }

        private void SpawnRandomPowerUp(Rectangle worldBounds)
        {
            var types = Enum.GetValues<PowerUpType>();
            var randomType = types[_random.Next(types.Length)];
            
            var position = new Vector2(
                _random.Next(50, worldBounds.Width - 50),
                _random.Next(50, worldBounds.Height - 50)
            );

            _powerUps.Add(new PowerUp(randomType, position));
        }

        public void ForceSpawnPowerUp(PowerUpType type, Vector2 position)
        {
            _powerUps.Add(new PowerUp(type, position));
        }
    }
}
