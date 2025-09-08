using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGame2DShooterPrototype.Source.UI
{
    public class GameBackground
    {
        private Texture2D _pixelTexture;
        private Texture2D _starTexture;
        private Vector2[] _stars;
        private float[] _starSpeeds;
        private Random _random;
        private GraphicsDevice _graphicsDevice;
        private float _time;

        public GameBackground(GraphicsDevice graphicsDevice, int starCount = 100)
        {
            _graphicsDevice = graphicsDevice;
            _random = new Random();

            // Create pixel texture
            _pixelTexture = new Texture2D(graphicsDevice, 1, 1);
            _pixelTexture.SetData(new[] { Color.White });

            // Create star texture (small white circle)
            _starTexture = new Texture2D(graphicsDevice, 3, 3);
            Color[] starData = new Color[9];
            starData[0] = Color.Transparent; starData[1] = Color.White; starData[2] = Color.Transparent;
            starData[3] = Color.White; starData[4] = Color.White; starData[5] = Color.White;
            starData[6] = Color.Transparent; starData[7] = Color.White; starData[8] = Color.Transparent;
            _starTexture.SetData(starData);

            // Initialize stars
            _stars = new Vector2[starCount];
            _starSpeeds = new float[starCount];
            InitializeStars();
        }

        private void InitializeStars()
        {
            for (int i = 0; i < _stars.Length; i++)
            {
                _stars[i] = new Vector2(
                    _random.Next(0, _graphicsDevice.Viewport.Width),
                    _random.Next(0, _graphicsDevice.Viewport.Height)
                );
                _starSpeeds[i] = 10 + (float)_random.NextDouble() * 30; // Speed between 10-40
            }
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _time += deltaTime;

            // Move stars downward
            for (int i = 0; i < _stars.Length; i++)
            {
                _stars[i].Y += _starSpeeds[i] * deltaTime;

                // Reset star position when it goes off screen
                if (_stars[i].Y > _graphicsDevice.Viewport.Height)
                {
                    _stars[i].Y = -5;
                    _stars[i].X = _random.Next(0, _graphicsDevice.Viewport.Width);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var viewport = _graphicsDevice.Viewport;

            // Draw gradient background
            DrawGradientBackground(spriteBatch, viewport);

            // Draw stars
            foreach (var star in _stars)
            {
                // Vary star brightness based on position and time for twinkling effect
                float brightness = 0.3f + 0.7f * (float)Math.Sin(_time * 2 + star.X * 0.01f);
                Color starColor = Color.White * brightness;
                spriteBatch.Draw(_starTexture, star, starColor);
            }

            // Draw subtle grid lines for space effect
            DrawSpaceGrid(spriteBatch, viewport);
        }

        private void DrawGradientBackground(SpriteBatch spriteBatch, Viewport viewport)
        {
            // Create a vertical gradient from dark blue to black
            int steps = 20;
            float stepHeight = viewport.Height / (float)steps;

            for (int i = 0; i < steps; i++)
            {
                float t = i / (float)(steps - 1);
                Color gradientColor = Color.Lerp(new Color(10, 20, 40), new Color(5, 10, 20), t);
                
                Rectangle rect = new Rectangle(
                    0, 
                    (int)(i * stepHeight), 
                    viewport.Width, 
                    (int)stepHeight + 1
                );
                
                spriteBatch.Draw(_pixelTexture, rect, gradientColor);
            }
        }

        private void DrawSpaceGrid(SpriteBatch spriteBatch, Viewport viewport)
        {
            // Draw subtle grid lines
            Color gridColor = new Color(20, 30, 50, 50); // Very subtle
            int gridSize = 100;

            // Vertical lines
            for (int x = 0; x < viewport.Width; x += gridSize)
            {
                Rectangle line = new Rectangle(x, 0, 1, viewport.Height);
                spriteBatch.Draw(_pixelTexture, line, gridColor);
            }

            // Horizontal lines
            for (int y = 0; y < viewport.Height; y += gridSize)
            {
                Rectangle line = new Rectangle(0, y, viewport.Width, 1);
                spriteBatch.Draw(_pixelTexture, line, gridColor);
            }
        }
    }
}
