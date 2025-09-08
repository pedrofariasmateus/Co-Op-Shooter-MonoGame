using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MonoGame2DShooterPrototype.Source.UI
{
    public class VisualEffects
    {
        private Texture2D _pixelTexture;
        private List<Particle> _particles;
        private Random _random;

        public VisualEffects(GraphicsDevice graphicsDevice)
        {
            _pixelTexture = new Texture2D(graphicsDevice, 1, 1);
            _pixelTexture.SetData(new[] { Color.White });
            _particles = new List<Particle>();
            _random = new Random();
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            for (int i = _particles.Count - 1; i >= 0; i--)
            {
                _particles[i].Update(deltaTime);
                if (_particles[i].IsExpired)
                {
                    _particles.RemoveAt(i);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var particle in _particles)
            {
                particle.Draw(spriteBatch, _pixelTexture);
            }
        }

        public void CreateExplosion(Vector2 position, Color color, int particleCount = 20)
        {
            for (int i = 0; i < particleCount; i++)
            {
                float angle = (float)(_random.NextDouble() * Math.PI * 2);
                float speed = 50 + (float)_random.NextDouble() * 100;
                Vector2 velocity = new Vector2(
                    (float)Math.Cos(angle) * speed,
                    (float)Math.Sin(angle) * speed
                );

                _particles.Add(new Particle
                {
                    Position = position,
                    Velocity = velocity,
                    Color = color,
                    Life = 1.0f,
                    MaxLife = 1.0f,
                    Size = 2 + (float)_random.NextDouble() * 3
                });
            }
        }

        public void CreateBulletTrail(Vector2 start, Vector2 end, Color color)
        {
            int trailCount = 5;
            for (int i = 0; i < trailCount; i++)
            {
                float t = i / (float)trailCount;
                Vector2 position = Vector2.Lerp(start, end, t);
                
                _particles.Add(new Particle
                {
                    Position = position,
                    Velocity = Vector2.Zero,
                    Color = color * (0.5f + 0.5f * (1 - t)),
                    Life = 0.2f,
                    MaxLife = 0.2f,
                    Size = 1f
                });
            }
        }

        public void CreateHitEffect(Vector2 position, Color color)
        {
            for (int i = 0; i < 8; i++)
            {
                float angle = (float)(i * Math.PI * 2 / 8);
                Vector2 velocity = new Vector2(
                    (float)Math.Cos(angle) * 30,
                    (float)Math.Sin(angle) * 30
                );

                _particles.Add(new Particle
                {
                    Position = position,
                    Velocity = velocity,
                    Color = color,
                    Life = 0.5f,
                    MaxLife = 0.5f,
                    Size = 1.5f
                });
            }
        }
    }

    public class Particle
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public Color Color;
        public float Life;
        public float MaxLife;
        public float Size;

        public bool IsExpired => Life <= 0;

        public void Update(float deltaTime)
        {
            Position += Velocity * deltaTime;
            Life -= deltaTime;
            
            // Fade out over time
            float alpha = Life / MaxLife;
            Color = Color * alpha;
            
            // Slow down over time
            Velocity *= 0.95f;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            if (Life > 0)
            {
                var rect = new Rectangle(
                    (int)(Position.X - Size/2), 
                    (int)(Position.Y - Size/2), 
                    (int)Size, 
                    (int)Size
                );
                spriteBatch.Draw(texture, rect, Color);
            }
        }
    }
}
