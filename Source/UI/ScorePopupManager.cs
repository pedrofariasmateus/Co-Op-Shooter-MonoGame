using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MonoGame2DShooterPrototype.Source.UI
{
    public class ScorePopup
    {
        public string Text { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Life { get; set; }
        public float MaxLife { get; set; }
        public Color Color { get; set; }
        public float Scale { get; set; }

        public bool IsExpired => Life <= 0;

        public void Update(float deltaTime)
        {
            Position += Velocity * deltaTime;
            Life -= deltaTime;
            
            // Fade out and scale down over time
            float alpha = Life / MaxLife;
            Color = Color * alpha;
            Scale = 1.0f + (1.0f - alpha) * 0.5f; // Scale up as it fades
            
            // Slow down velocity over time
            Velocity *= 0.95f;
        }
    }

    public class ScorePopupManager
    {
        private List<ScorePopup> _popups;
        private SpriteFont _font;
        private Random _random;

        public ScorePopupManager(SpriteFont font)
        {
            _font = font;
            _popups = new List<ScorePopup>();
            _random = new Random();
        }

        public void AddScorePopup(Vector2 position, int score, Color color)
        {
            _popups.Add(new ScorePopup
            {
                Text = $"+{score}",
                Position = position,
                Velocity = new Vector2(
                    (_random.NextSingle() - 0.5f) * 50f, // Random horizontal drift
                    -50f - _random.NextSingle() * 30f    // Upward movement
                ),
                Life = 2.0f,
                MaxLife = 2.0f,
                Color = color,
                Scale = 1.0f
            });
        }

        public void AddTextPopup(Vector2 position, string text, Color color, float life = 1.5f)
        {
            _popups.Add(new ScorePopup
            {
                Text = text,
                Position = position,
                Velocity = new Vector2(0, -30f),
                Life = life,
                MaxLife = life,
                Color = color,
                Scale = 1.0f
            });
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            for (int i = _popups.Count - 1; i >= 0; i--)
            {
                _popups[i].Update(deltaTime);
                if (_popups[i].IsExpired)
                {
                    _popups.RemoveAt(i);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var popup in _popups)
            {
                var textSize = _font.MeasureString(popup.Text);
                var origin = textSize / 2;
                
                spriteBatch.DrawString(_font, popup.Text, popup.Position, popup.Color, 
                    0f, origin, popup.Scale, SpriteEffects.None, 0f);
            }
        }

        public void Clear()
        {
            _popups.Clear();
        }
    }
}
