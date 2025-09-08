using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGame2DShooterPrototype.Source.UI
{
    public enum EnemyType
    {
        Basic,
        Fast,
        Heavy,
        Boss
    }

    public static class EnemyVisuals
    {
        public static Color GetEnemyColor(EnemyType type)
        {
            return type switch
            {
                EnemyType.Basic => Color.Red,
                EnemyType.Fast => Color.Orange,
                EnemyType.Heavy => Color.DarkRed,
                EnemyType.Boss => Color.Purple,
                _ => Color.Red
            };
        }

        public static Vector2 GetEnemySize(EnemyType type)
        {
            return type switch
            {
                EnemyType.Basic => new Vector2(32, 32),
                EnemyType.Fast => new Vector2(24, 24),
                EnemyType.Heavy => new Vector2(48, 48),
                EnemyType.Boss => new Vector2(64, 64),
                _ => new Vector2(32, 32)
            };
        }

        public static void DrawEnhancedEnemy(SpriteBatch spriteBatch, Texture2D pixelTexture, Vector2 position, EnemyType type, float health = 1.0f)
        {
            var size = GetEnemySize(type);
            var color = GetEnemyColor(type);
            var rect = new Rectangle((int)(position.X - size.X/2), (int)(position.Y - size.Y/2), (int)size.X, (int)size.Y);
            
            // Draw enemy body
            spriteBatch.Draw(pixelTexture, rect, color);
            
            // Draw health bar for heavy and boss enemies
            if (type == EnemyType.Heavy || type == EnemyType.Boss)
            {
                var healthBarBg = new Rectangle(rect.X, rect.Y - 8, rect.Width, 4);
                var healthBar = new Rectangle(rect.X, rect.Y - 8, (int)(rect.Width * health), 4);
                
                spriteBatch.Draw(pixelTexture, healthBarBg, Color.DarkRed);
                spriteBatch.Draw(pixelTexture, healthBar, Color.Green);
            }
            
            // Add visual effects for different types
            switch (type)
            {
                case EnemyType.Fast:
                    // Draw speed lines
                    var speedLine1 = new Rectangle(rect.X - 10, rect.Y + rect.Height/3, 8, 2);
                    var speedLine2 = new Rectangle(rect.X - 10, rect.Y + 2*rect.Height/3, 8, 2);
                    spriteBatch.Draw(pixelTexture, speedLine1, Color.Yellow * 0.7f);
                    spriteBatch.Draw(pixelTexture, speedLine2, Color.Yellow * 0.7f);
                    break;
                    
                case EnemyType.Heavy:
                    // Draw armor plating
                    var armor1 = new Rectangle(rect.X + 4, rect.Y + 4, rect.Width - 8, 4);
                    var armor2 = new Rectangle(rect.X + 4, rect.Y + rect.Height - 8, rect.Width - 8, 4);
                    spriteBatch.Draw(pixelTexture, armor1, Color.Gray);
                    spriteBatch.Draw(pixelTexture, armor2, Color.Gray);
                    break;
                    
                case EnemyType.Boss:
                    // Draw pulsing aura
                    float pulseAlpha = 0.3f + 0.2f * (float)Math.Sin(DateTime.Now.Millisecond * 0.01);
                    var aura = new Rectangle(rect.X - 8, rect.Y - 8, rect.Width + 16, rect.Height + 16);
                    spriteBatch.Draw(pixelTexture, aura, Color.Purple * pulseAlpha);
                    break;
            }
        }
    }

    public class EnhancedGameplayHUD
    {
        private Texture2D _pixelTexture;
        private SpriteFont _font;
        private GraphicsDevice _graphicsDevice;

        public EnhancedGameplayHUD(GraphicsDevice graphicsDevice, SpriteFont font)
        {
            _graphicsDevice = graphicsDevice;
            _font = font;
            _pixelTexture = new Texture2D(graphicsDevice, 1, 1);
            _pixelTexture.SetData(new[] { Color.White });
        }

        public void DrawWaveProgressBar(SpriteBatch spriteBatch, int enemiesRemaining, int totalEnemies)
        {
            var viewport = _graphicsDevice.Viewport;
            var progressBarBg = new Rectangle(viewport.Width/2 - 150, 30, 300, 20);
            var progressBar = new Rectangle(viewport.Width/2 - 150, 30, 
                (int)(300 * (1.0f - (float)enemiesRemaining / totalEnemies)), 20);
            
            // Background
            spriteBatch.Draw(_pixelTexture, progressBarBg, Color.DarkGray);
            // Progress
            spriteBatch.Draw(_pixelTexture, progressBar, Color.Gold);
            // Border
            DrawBorder(spriteBatch, progressBarBg, Color.White, 2);
            
            // Text
            string progressText = $"Wave Progress: {totalEnemies - enemiesRemaining}/{totalEnemies}";
            var textSize = _font.MeasureString(progressText);
            spriteBatch.DrawString(_font, progressText, 
                new Vector2(viewport.Width/2 - textSize.X/2, 35), Color.White);
        }

        public void DrawPowerUpNotification(SpriteBatch spriteBatch, string powerUpName, float showTime)
        {
            if (showTime <= 0) return;
            
            var viewport = _graphicsDevice.Viewport;
            float alpha = Math.Min(1.0f, showTime * 2); // Fade in quickly
            
            string notificationText = $"POWER-UP: {powerUpName}";
            var textSize = _font.MeasureString(notificationText);
            var position = new Vector2(viewport.Width/2 - textSize.X/2, viewport.Height/2 - 100);
            
            // Background panel
            var panel = new Rectangle((int)position.X - 20, (int)position.Y - 10, 
                (int)textSize.X + 40, (int)textSize.Y + 20);
            spriteBatch.Draw(_pixelTexture, panel, Color.Black * 0.7f * alpha);
            DrawBorder(spriteBatch, panel, Color.Gold, 2);
            
            // Text with glow effect
            spriteBatch.DrawString(_font, notificationText, position + Vector2.One, Color.Black * alpha);
            spriteBatch.DrawString(_font, notificationText, position, Color.Gold * alpha);
        }

        public void DrawKillStreak(SpriteBatch spriteBatch, int killStreak, Vector2 position)
        {
            if (killStreak < 3) return; // Only show for streaks of 3+
            
            string streakText = $"KILL STREAK: {killStreak}";
            Color streakColor = killStreak >= 10 ? Color.Red : killStreak >= 7 ? Color.Orange : Color.Yellow;
            float scale = 1.0f + (killStreak / 20.0f); // Scale grows with streak
            
            var textSize = _font.MeasureString(streakText);
            var origin = textSize / 2;
            
            // Draw with scaling and pulsing
            float pulse = 1.0f + 0.2f * (float)Math.Sin(DateTime.Now.Millisecond * 0.02);
            
            spriteBatch.DrawString(_font, streakText, position, streakColor, 0f, origin, scale * pulse, SpriteEffects.None, 0f);
        }

        private void DrawBorder(SpriteBatch spriteBatch, Rectangle bounds, Color color, int thickness)
        {
            // Top
            spriteBatch.Draw(_pixelTexture, new Rectangle(bounds.X, bounds.Y, bounds.Width, thickness), color);
            // Bottom  
            spriteBatch.Draw(_pixelTexture, new Rectangle(bounds.X, bounds.Y + bounds.Height - thickness, bounds.Width, thickness), color);
            // Left
            spriteBatch.Draw(_pixelTexture, new Rectangle(bounds.X, bounds.Y, thickness, bounds.Height), color);
            // Right
            spriteBatch.Draw(_pixelTexture, new Rectangle(bounds.X + bounds.Width - thickness, bounds.Y, thickness, bounds.Height), color);
        }
    }
}
