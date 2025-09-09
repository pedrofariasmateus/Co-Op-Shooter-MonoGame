using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGame2DShooterPrototype.Source.UI
{
    public class GameHUD
    {
        private SpriteFont _font;
        private SpriteFont _titleFont;
        private Texture2D _pixelTexture;
        private GraphicsDevice _graphicsDevice;

        // HUD colors
        private readonly Color _hudBackgroundColor = new Color(0, 0, 0, 180);
        private readonly Color _hudBorderColor = Color.White;
        private readonly Color _scoreColor = Color.Gold;
        private readonly Color _counterColor = Color.Cyan;
        private readonly Color _warningColor = Color.Red;
        private readonly Viewport _screenView;

        public GameHUD(GraphicsDevice graphicsDevice, SpriteFont font, SpriteFont titleFont = null)
        {
            _graphicsDevice = graphicsDevice;
            _font = font;
            _titleFont = titleFont ?? font;
            _screenView = _graphicsDevice.Viewport;

            // Create a 1x1 white pixel texture for drawing rectangles
            _pixelTexture = new Texture2D(graphicsDevice, 1, 1);
            _pixelTexture.SetData(new[] { Color.White });
        }

        public void Draw(SpriteBatch spriteBatch, int score, int bulletCount, int enemyCount, bool testMode = false, 
            float playerHealth = 100f, int wave = 1, float timeRemaining = 0f)
        {
            
            // Draw main HUD panel with better organization
            DrawMainInfoPanel(spriteBatch, score, bulletCount, enemyCount, playerHealth, wave, timeRemaining);

            // Draw test mode indicator
            if (testMode)
            {
                DrawTestModeIndicator(spriteBatch);
                // Draw performance warnings with better styling
                DrawPerformanceWarnings(spriteBatch, bulletCount);
            }
        }

        private void DrawMainInfoPanel(SpriteBatch spriteBatch, int score, int bulletCount, int enemyCount, 
            float playerHealth, int wave, float timeRemaining)
        {
            // Main HUD panel with better sizing - made much taller to accommodate all elements with proper spacing
            var mainPanel = new Rectangle((int)(_screenView.Width*0.01f), (int)(_screenView.Height*0.01f), 320, 220);
            
            // Draw background with gradient effect
            spriteBatch.Draw(_pixelTexture, mainPanel, new Color(0, 0, 0, 200));
            DrawHUDPanel(spriteBatch, mainPanel);
            
            // Score with smaller, more appropriate font size
            string scoreText = $"SCORE: {score}";
            spriteBatch.DrawString(_font, scoreText, new Vector2(20, 25), _scoreColor);
            
            // Wave information - positioned better
            string waveText = $"WAVE: {wave}";
            spriteBatch.DrawString(_font, waveText, new Vector2(200, 25), Color.Orange);
            
            // Entity counters with icons - increased spacing between rows
            DrawCounterWithIcon(spriteBatch, "BULLETS", bulletCount, new Vector2(20, 55), _counterColor, Color.Yellow);
            DrawCounterWithIcon(spriteBatch, "ENEMIES", enemyCount, new Vector2(20, 80), _counterColor, Color.Red);
            
            // Enhanced health bar - positioned with more space after counters and made more visible
            DrawHealthBar(spriteBatch, new Vector2(20, 110), playerHealth, 100f, new Vector2(120, 18));
            spriteBatch.DrawString(_font, "HEALTH", new Vector2(150, 112), Color.White);
            
            // Health percentage - repositioned to accommodate larger health bar
            string healthPercent = $"{playerHealth:F0}%";
            spriteBatch.DrawString(_font, healthPercent, new Vector2(250, 112), 
                playerHealth > 50 ? Color.Green : playerHealth > 25 ? Color.Yellow : Color.Red);
            
            // Time remaining with urgency indicator - more space below health
            if (timeRemaining > 0)
            {
                Color timeColor = timeRemaining < 10 ? Color.Red : timeRemaining < 30 ? Color.Yellow : Color.Cyan;
                string timeText = $"TIME: {timeRemaining:F1}s";
                spriteBatch.DrawString(_font, timeText, new Vector2(20, 140), timeColor);
                
                // Time bar - positioned well below text to prevent overlap
                if (timeRemaining <= 60) // Show bar for last minute
                {
                    var timeBar = new Rectangle(20, 165, (int)(120 * (timeRemaining / 60f)), 10);
                    var timeBg = new Rectangle(20, 165, 120, 10);
                    spriteBatch.Draw(_pixelTexture, timeBg, Color.DarkGray);
                    spriteBatch.Draw(_pixelTexture, timeBar, timeColor);
                }
            }
        }

        private void DrawCounterWithIcon(SpriteBatch spriteBatch, string label, int count, Vector2 position, Color textColor, Color iconColor)
        {
            // Simple icon (small rectangle)
            var icon = new Rectangle((int)position.X, (int)position.Y + 3, 8, 8);
            spriteBatch.Draw(_pixelTexture, icon, iconColor);
            
            // Counter text with proper spacing
            string counterText = $"{label}: {count}";
            spriteBatch.DrawString(_font, counterText, new Vector2(position.X + 15, position.Y), textColor);
        }

        private void DrawPerformanceWarnings(SpriteBatch spriteBatch, int bulletCount)
        {
            if (bulletCount > 50)
            {
                var warningPanel = new Rectangle(10, 240, 180, 25);
                spriteBatch.Draw(_pixelTexture, warningPanel, new Color(100, 0, 0, 150));
                DrawHUDPanel(spriteBatch, warningPanel);
                
                DrawWarning(spriteBatch, "HIGH BULLET COUNT", new Vector2(15, 245));
            }
        }

        private void DrawHUDPanel(SpriteBatch spriteBatch, Rectangle bounds)
        {
            // Draw background
            spriteBatch.Draw(_pixelTexture, bounds, _hudBackgroundColor);
            
            // Draw border
            int borderThickness = 2;
            // Top
            spriteBatch.Draw(_pixelTexture, new Rectangle(bounds.X, bounds.Y, bounds.Width, borderThickness), _hudBorderColor);
            // Bottom  
            spriteBatch.Draw(_pixelTexture, new Rectangle(bounds.X, bounds.Y + bounds.Height - borderThickness, bounds.Width, borderThickness), _hudBorderColor);
            // Left
            spriteBatch.Draw(_pixelTexture, new Rectangle(bounds.X, bounds.Y, borderThickness, bounds.Height), _hudBorderColor);
            // Right
            spriteBatch.Draw(_pixelTexture, new Rectangle(bounds.X + bounds.Width - borderThickness, bounds.Y, borderThickness, bounds.Height), _hudBorderColor);
        }

        private void DrawTestModeIndicator(SpriteBatch spriteBatch)
        {
            var testModePanel = new Rectangle(_screenView.Width - 130, 10, 120, 35);
            DrawHUDPanel(spriteBatch, testModePanel);
            
            string testText = "TEST MODE";
            var textSize = _font.MeasureString(testText);
            var textPos = new Vector2(
                testModePanel.X + (testModePanel.Width - textSize.X) / 2,
                testModePanel.Y + (testModePanel.Height - textSize.Y) / 2
            );
            
            spriteBatch.DrawString(_font, testText, textPos, _warningColor);
        }

        private void DrawWarning(SpriteBatch spriteBatch, string message, Vector2 position)
        {
            // Add a subtle pulsing effect
            float alpha = 0.7f + 0.3f * (float)Math.Sin(DateTime.Now.Millisecond * 0.01);
            Color warningColor = _warningColor * alpha;
            
            spriteBatch.DrawString(_font, message, position, warningColor);
        }

        public void DrawCrosshair(SpriteBatch spriteBatch, Vector2 center, Color color)
        {
            int size = 10;
            int thickness = 2;
            
            // Horizontal line
            spriteBatch.Draw(_pixelTexture, 
                new Rectangle((int)center.X - size, (int)center.Y - thickness/2, size * 2, thickness), 
                color);
            
            // Vertical line
            spriteBatch.Draw(_pixelTexture, 
                new Rectangle((int)center.X - thickness/2, (int)center.Y - size, thickness, size * 2), 
                color);
        }

        public void DrawHealthBar(SpriteBatch spriteBatch, Vector2 position, float currentHealth, float maxHealth, Vector2 size)
        {
            var backgroundRect = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            var healthRect = new Rectangle((int)position.X, (int)position.Y, 
                (int)(size.X * (currentHealth / maxHealth)), (int)size.Y);

            // Background - more visible dark background
            spriteBatch.Draw(_pixelTexture, backgroundRect, new Color(60, 0, 0, 255));
            
            // Health bar with better visibility and contrast
            float healthPercentage = currentHealth / maxHealth;
            Color healthColor = healthPercentage > 0.6f ? Color.LimeGreen : 
                               healthPercentage > 0.3f ? Color.Orange : Color.Red;
            spriteBatch.Draw(_pixelTexture, healthRect, healthColor);
            
            // Enhanced border for better visibility
            DrawHUDPanel(spriteBatch, backgroundRect);
        }

        public void DrawComboIndicator(SpriteBatch spriteBatch, int comboCount, Vector2 position)
        {
            if (comboCount > 1)
            {
                string comboText = $"COMBO x{comboCount}";
                float scale = 1.0f + (comboCount * 0.1f); // Scale grows with combo
                Color comboColor = Color.Lerp(Color.Yellow, Color.Red, Math.Min(comboCount / 10f, 1f));
                
                var textSize = _font.MeasureString(comboText);
                var origin = textSize / 2;
                
                spriteBatch.DrawString(_font, comboText, position, comboColor, 0f, origin, scale, SpriteEffects.None, 0f);
            }
        }

        public void DrawScorePopup(SpriteBatch spriteBatch, string text, Vector2 position, float alpha)
        {
            Color popupColor = Color.Gold * alpha;
            float scale = 1.5f * alpha; // Fade and shrink
            var textSize = _font.MeasureString(text);
            var origin = textSize / 2;
            
            spriteBatch.DrawString(_font, text, position, popupColor, 0f, origin, scale, SpriteEffects.None, 0f);
        }

        public void DrawMiniMap(SpriteBatch spriteBatch, Vector2 playerPos, System.Collections.Generic.List<Vector2> enemyPositions, Rectangle worldBounds)
        {
            var viewport = _graphicsDevice.Viewport;
            var miniMapSize = new Vector2(140, 90); // Increased size for better visibility
            var miniMapPos = new Vector2(viewport.Width - miniMapSize.X - 10, viewport.Height - miniMapSize.Y - 25);
            var miniMapRect = new Rectangle((int)miniMapPos.X, (int)miniMapPos.Y, (int)miniMapSize.X, (int)miniMapSize.Y);
            
            // Draw mini-map background with better styling
            spriteBatch.Draw(_pixelTexture, miniMapRect, new Color(5, 15, 25, 220)); // Darker, more opaque background
            DrawHUDPanel(spriteBatch, miniMapRect);
            
            // Draw grid lines for better visual reference
            for (int i = 1; i < 3; i++)
            {
                int gridX = (int)(miniMapPos.X + i * miniMapSize.X / 3);
                int gridY = (int)(miniMapPos.Y + i * miniMapSize.Y / 3);
                
                // Vertical grid lines
                spriteBatch.Draw(_pixelTexture, new Rectangle(gridX, (int)miniMapPos.Y, 1, (int)miniMapSize.Y), Color.Gray * 0.3f);
                // Horizontal grid lines
                spriteBatch.Draw(_pixelTexture, new Rectangle((int)miniMapPos.X, gridY, (int)miniMapSize.X, 1), Color.Gray * 0.3f);
            }
            
            // Draw player dot with enhanced visibility
            var playerMapPos = new Vector2(
                miniMapPos.X + (playerPos.X / worldBounds.Width) * miniMapSize.X,
                miniMapPos.Y + (playerPos.Y / worldBounds.Height) * miniMapSize.Y
            );
            
            // Player dot (larger and with outline for better visibility)
            var playerOutline = new Rectangle((int)playerMapPos.X - 3, (int)playerMapPos.Y - 3, 6, 6);
            var playerDot = new Rectangle((int)playerMapPos.X - 2, (int)playerMapPos.Y - 2, 4, 4);
            spriteBatch.Draw(_pixelTexture, playerOutline, Color.White);
            spriteBatch.Draw(_pixelTexture, playerDot, Color.Lime);
            
            // Enemy dots with better visibility
            foreach (var enemyPos in enemyPositions)
            {
                var enemyMapPos = new Vector2(
                    miniMapPos.X + (enemyPos.X / worldBounds.Width) * miniMapSize.X,
                    miniMapPos.Y + (enemyPos.Y / worldBounds.Height) * miniMapSize.Y
                );
                
                var enemyDot = new Rectangle((int)enemyMapPos.X - 1, (int)enemyMapPos.Y - 1, 3, 3);
                spriteBatch.Draw(_pixelTexture, enemyDot, Color.Red);
            }
            
            // Enhanced label with better positioning and background
            var labelPos = new Vector2(miniMapPos.X, miniMapPos.Y - 22);
            var labelBg = new Rectangle((int)labelPos.X - 2, (int)labelPos.Y - 2, 50, 18);
            spriteBatch.Draw(_pixelTexture, labelBg, new Color(0, 0, 0, 150));
            spriteBatch.DrawString(_font, "RADAR", labelPos, Color.Cyan);
        }

        public void DrawPlayerStatusIndicators(SpriteBatch spriteBatch, Vector2 playerVelocity, bool isMoving, bool isShooting)
        {
            var viewport = _graphicsDevice.Viewport;
            
            // Movement indicator
            if (isMoving)
            {
                var movementPanel = new Rectangle(10, 160, 120, 40);
                DrawHUDPanel(spriteBatch, movementPanel);
                
                float speed = playerVelocity.Length();
                string speedText = $"Speed: {speed:F0}";
                spriteBatch.DrawString(_font, speedText, new Vector2(20, 170), Color.Cyan);
                
                // Speed bar
                var speedBar = new Rectangle(20, 185, Math.Min(100, (int)speed), 5);
                var speedBackground = new Rectangle(20, 185, 100, 5);
                spriteBatch.Draw(_pixelTexture, speedBackground, Color.DarkGray);
                spriteBatch.Draw(_pixelTexture, speedBar, Color.Cyan);
            }
            
            // Shooting indicator
            if (isShooting)
            {
                var shootingIndicator = new Rectangle(viewport.Width - 80, viewport.Height - 50, 60, 30);
                DrawHUDPanel(spriteBatch, shootingIndicator);
                spriteBatch.DrawString(_font, "FIRING", new Vector2(shootingIndicator.X + 5, shootingIndicator.Y + 8), Color.Red);
            }
        }

        public void DrawWeaponInfo(SpriteBatch spriteBatch, string weaponName, int ammo, float fireRate)
        {
            var viewport = _graphicsDevice.Viewport;
            var weaponPanel = new Rectangle(viewport.Width - 200, 60, 190, 90); // Increased width for better text fit
            DrawHUDPanel(spriteBatch, weaponPanel);
            
            // Weapon name
            spriteBatch.DrawString(_font, weaponName, new Vector2(weaponPanel.X + 8, weaponPanel.Y + 8), Color.White);
            
            // Ammo count
            string ammoText = ammo >= 0 ? $"Ammo: {ammo}" : "Ammo: Unlimited";
            Color ammoColor = ammo < 10 && ammo >= 0 ? Color.Red : Color.White;
            spriteBatch.DrawString(_font, ammoText, new Vector2(weaponPanel.X + 8, weaponPanel.Y + 28), ammoColor);
            
            // Fire rate label
            spriteBatch.DrawString(_font, "Fire Rate", new Vector2(weaponPanel.X + 8, weaponPanel.Y + 48), Color.Gray);
            
            // Fire rate indicator (visual bar) - better positioned
            int barWidth = Math.Max(5, (int)(fireRate * 140)); // Increased bar max width to fit new panel
            var fireRateBar = new Rectangle(weaponPanel.X + 8, weaponPanel.Y + 66, barWidth, 8);
            var fireRateBackground = new Rectangle(weaponPanel.X + 8, weaponPanel.Y + 66, 140, 8);
            
            // Background bar
            spriteBatch.Draw(_pixelTexture, fireRateBackground, Color.DarkGray);
            // Fire rate bar
            spriteBatch.Draw(_pixelTexture, fireRateBar, Color.Green);
        }

        public void DrawPowerUpIndicator(SpriteBatch spriteBatch, string powerUpName, float timeRemaining)
        {
            if (timeRemaining <= 0) return;
            
            var viewport = _graphicsDevice.Viewport;
            var powerUpPanel = new Rectangle(viewport.Width - 200, 150, 180, 50);
            DrawHUDPanel(spriteBatch, powerUpPanel);
            
            Color powerUpColor = timeRemaining < 5f ? Color.Red : Color.Green;
            
            spriteBatch.DrawString(_font, powerUpName, new Vector2(powerUpPanel.X + 10, powerUpPanel.Y + 10), powerUpColor);
            spriteBatch.DrawString(_font, $"Time: {timeRemaining:F1}s", new Vector2(powerUpPanel.X + 10, powerUpPanel.Y + 30), powerUpColor);
        }

        public void DrawAdvancedCrosshair(SpriteBatch spriteBatch, Vector2 center, Color color, float accuracy = 1.0f)
        {
            int baseSize = 15;
            int size = (int)(baseSize / accuracy); // Crosshair expands with lower accuracy
            int thickness = 2;
            int gap = 5; // Gap in the center
            
            // Top line
            spriteBatch.Draw(_pixelTexture, 
                new Rectangle((int)center.X - thickness/2, (int)center.Y - size, thickness, size - gap), 
                color);
            
            // Bottom line
            spriteBatch.Draw(_pixelTexture, 
                new Rectangle((int)center.X - thickness/2, (int)center.Y + gap, thickness, size - gap), 
                color);
            
            // Left line
            spriteBatch.Draw(_pixelTexture, 
                new Rectangle((int)center.X - size, (int)center.Y - thickness/2, size - gap, thickness), 
                color);
            
            // Right line
            spriteBatch.Draw(_pixelTexture, 
                new Rectangle((int)center.X + gap, (int)center.Y - thickness/2, size - gap, thickness), 
                color);
            
            // Center dot for precision
            spriteBatch.Draw(_pixelTexture, 
                new Rectangle((int)center.X - 1, (int)center.Y - 1, 2, 2), 
                color);
        }

        public void DrawGameOverScreen(SpriteBatch spriteBatch, int finalScore, int wave)
        {
            var viewport = _graphicsDevice.Viewport;
            
            // Semi-transparent overlay
            spriteBatch.Draw(_pixelTexture, new Rectangle(0, 0, viewport.Width, viewport.Height), Color.Black * 0.7f);
            
            // Game Over panel
            var gameOverPanel = new Rectangle(viewport.Width/2 - 200, viewport.Height/2 - 100, 400, 200);
            DrawHUDPanel(spriteBatch, gameOverPanel);
            
            // Game Over text
            string gameOverText = "GAME OVER";
            var textSize = _titleFont.MeasureString(gameOverText);
            var textPos = new Vector2(viewport.Width/2 - textSize.X/2, gameOverPanel.Y + 20);
            spriteBatch.DrawString(_titleFont, gameOverText, textPos, Color.Red);
            
            // Final score
            string scoreText = $"Final Score: {finalScore}";
            textSize = _font.MeasureString(scoreText);
            textPos = new Vector2(viewport.Width/2 - textSize.X/2, gameOverPanel.Y + 70);
            spriteBatch.DrawString(_font, scoreText, textPos, Color.Gold);
            
            // Wave reached
            string waveText = $"Wave Reached: {wave}";
            textSize = _font.MeasureString(waveText);
            textPos = new Vector2(viewport.Width/2 - textSize.X/2, gameOverPanel.Y + 100);
            spriteBatch.DrawString(_font, waveText, textPos, Color.White);
            
            // Restart instruction
            string restartText = "Press R to Restart";
            textSize = _font.MeasureString(restartText);
            textPos = new Vector2(viewport.Width/2 - textSize.X/2, gameOverPanel.Y + 150);
            spriteBatch.DrawString(_font, restartText, textPos, Color.Yellow);
        }

        public void DrawPauseScreen(SpriteBatch spriteBatch)
        {
            var viewport = _graphicsDevice.Viewport;

            // Semi-transparent overlay
            spriteBatch.Draw(_pixelTexture, new Rectangle(0, 0, viewport.Width, viewport.Height), Color.Black * 0.5f);

            // Pause text
            string pauseText = "PAUSED";
            var textSize = _titleFont.MeasureString(pauseText);
            var textPos = new Vector2(viewport.Width / 2 - textSize.X / 2, viewport.Height / 2 - textSize.Y / 2);
            spriteBatch.DrawString(_titleFont, pauseText, textPos, Color.White);

            // Resume instruction
            string resumeText = "Press P to Resume";
            textSize = _font.MeasureString(resumeText);
            textPos = new Vector2(viewport.Width / 2 - textSize.X / 2, viewport.Height / 2 + 50);
            spriteBatch.DrawString(_font, resumeText, textPos, Color.Gray);
        }
        public void DrawTargetLockOn(SpriteBatch spriteBatch, Vector2 targetPosition, float distance, bool isLocked)
        {
            if (!isLocked) return;

            // Draw targeting brackets around the enemy
            int size = 20;
            int thickness = 2;
            Color targetColor = Color.Red;

            // Top-left bracket
            spriteBatch.Draw(_pixelTexture,
                new Rectangle((int)targetPosition.X - size, (int)targetPosition.Y - size, size / 2, thickness), targetColor);
            spriteBatch.Draw(_pixelTexture,
                new Rectangle((int)targetPosition.X - size, (int)targetPosition.Y - size, thickness, size / 2), targetColor);

            // Top-right bracket
            spriteBatch.Draw(_pixelTexture,
                new Rectangle((int)targetPosition.X + size / 2, (int)targetPosition.Y - size, size / 2, thickness), targetColor);
            spriteBatch.Draw(_pixelTexture,
                new Rectangle((int)targetPosition.X + size, (int)targetPosition.Y - size, thickness, size / 2), targetColor);

            // Bottom-left bracket
            spriteBatch.Draw(_pixelTexture,
                new Rectangle((int)targetPosition.X - size, (int)targetPosition.Y + size / 2, size / 2, thickness), targetColor);
            spriteBatch.Draw(_pixelTexture,
                new Rectangle((int)targetPosition.X - size, (int)targetPosition.Y + size / 2, thickness, size / 2), targetColor);

            // Bottom-right bracket
            spriteBatch.Draw(_pixelTexture,
                new Rectangle((int)targetPosition.X + size / 2, (int)targetPosition.Y + size, size / 2, thickness), targetColor);
            spriteBatch.Draw(_pixelTexture,
                new Rectangle((int)targetPosition.X + size, (int)targetPosition.Y + size / 2, thickness, size / 2), targetColor);

            // Distance indicator
            string distanceText = $"{distance:F0}m";
            var textSize = _font.MeasureString(distanceText);
            spriteBatch.DrawString(_font, distanceText,
                new Vector2(targetPosition.X - textSize.X / 2, targetPosition.Y + size + 10), Color.Red);
        }

        public void DrawAccuracyIndicator(SpriteBatch spriteBatch, int hits, int totalShots)
        {
            if (totalShots == 0) return;
            
            var viewport = _graphicsDevice.Viewport;
            float accuracy = (float)hits / totalShots;
            
            var accuracyPanel = new Rectangle(10, 210, 150, 50);
            DrawHUDPanel(spriteBatch, accuracyPanel);
            
            string accuracyText = $"Accuracy: {accuracy:P0}";
            Color accuracyColor = accuracy > 0.7f ? Color.Green : accuracy > 0.4f ? Color.Yellow : Color.Red;
            
            spriteBatch.DrawString(_font, accuracyText, new Vector2(20, 220), accuracyColor);
            spriteBatch.DrawString(_font, $"Hits: {hits}/{totalShots}", new Vector2(20, 240), Color.White);
        }

        public void DrawCentralHUD(SpriteBatch spriteBatch, int score, int wave, float waveProgress, int enemiesRemaining)
        {
            var viewport = _graphicsDevice.Viewport;
            
            // Central wave progress bar
            var progressBarBg = new Rectangle(viewport.Width/2 - 200, 20, 400, 25);
            var progressBar = new Rectangle(viewport.Width/2 - 200, 20, (int)(400 * waveProgress), 25);
            
            // Draw background
            spriteBatch.Draw(_pixelTexture, progressBarBg, new Color(20, 20, 20, 200));
            // Draw progress
            Color progressColor = waveProgress > 0.8f ? Color.Gold : waveProgress > 0.5f ? Color.Orange : Color.Red;
            spriteBatch.Draw(_pixelTexture, progressBar, progressColor);
            // Draw border
            DrawHUDPanel(spriteBatch, progressBarBg);
            
            // Wave text
            string waveText = $"WAVE {wave}";
            var waveTextSize = _titleFont.MeasureString(waveText);
            spriteBatch.DrawString(_titleFont, waveText, 
                new Vector2(viewport.Width/2 - waveTextSize.X/2, 25), Color.White);
            
            // Enemies remaining
            string enemiesText = $"Enemies: {enemiesRemaining}";
            var enemiesTextSize = _font.MeasureString(enemiesText);
            spriteBatch.DrawString(_font, enemiesText, 
                new Vector2(viewport.Width/2 - enemiesTextSize.X/2, 50), Color.Cyan);
        }

        public void DrawNotificationPanel(SpriteBatch spriteBatch, string message, Color color, float duration, float maxDuration)
        {
            if (duration <= 0) return;
            
            var viewport = _graphicsDevice.Viewport;
            float alpha = Math.Min(1.0f, duration / (maxDuration * 0.3f)); // Fade in/out effect
            
            var panel = new Rectangle(viewport.Width/2 - 150, viewport.Height - 120, 300, 60);
            
            // Background with fade
            spriteBatch.Draw(_pixelTexture, panel, new Color(0, 0, 0, 150) * alpha);
            DrawHUDPanel(spriteBatch, panel);
            
            // Message text
            var textSize = _font.MeasureString(message);
            var textPos = new Vector2(
                panel.X + (panel.Width - textSize.X) / 2,
                panel.Y + (panel.Height - textSize.Y) / 2
            );
            
            spriteBatch.DrawString(_font, message, textPos, color * alpha);
        }

        public void DrawAdvancedMiniMap(SpriteBatch spriteBatch, Vector2 playerPos, System.Collections.Generic.List<Vector2> enemyPositions, 
            System.Collections.Generic.List<Vector2> powerUpPositions, Rectangle worldBounds)
        {
            var viewport = _graphicsDevice.Viewport;
            var miniMapSize = new Vector2(180, 120); // Larger mini-map
            var miniMapPos = new Vector2(viewport.Width - miniMapSize.X - 15, viewport.Height - miniMapSize.Y - 15);
            var miniMapRect = new Rectangle((int)miniMapPos.X, (int)miniMapPos.Y, (int)miniMapSize.X, (int)miniMapSize.Y);
            
            // Draw mini-map background with better styling
            spriteBatch.Draw(_pixelTexture, miniMapRect, new Color(10, 20, 30, 200));
            DrawHUDPanel(spriteBatch, miniMapRect);
            
            // Draw grid lines for better reference
            for (int i = 1; i < 4; i++)
            {
                int x = (int)(miniMapPos.X + i * miniMapSize.X / 4);
                int y = (int)(miniMapPos.Y + i * miniMapSize.Y / 4);
                
                // Vertical lines
                spriteBatch.Draw(_pixelTexture, new Rectangle(x, (int)miniMapPos.Y, 1, (int)miniMapSize.Y), Color.Gray * 0.3f);
                // Horizontal lines
                spriteBatch.Draw(_pixelTexture, new Rectangle((int)miniMapPos.X, y, (int)miniMapSize.X, 1), Color.Gray * 0.3f);
            }
            
            // Draw player with direction indicator
            var playerMapPos = new Vector2(
                miniMapPos.X + (playerPos.X / worldBounds.Width) * miniMapSize.X,
                miniMapPos.Y + (playerPos.Y / worldBounds.Height) * miniMapSize.Y
            );
            
            // Player dot (larger and with pulse effect)
            float pulse = 0.8f + 0.2f * (float)Math.Sin(DateTime.Now.Millisecond * 0.01);
            var playerDot = new Rectangle((int)playerMapPos.X - 3, (int)playerMapPos.Y - 3, 6, 6);
            spriteBatch.Draw(_pixelTexture, playerDot, Color.Lime * pulse);
            
            // Enemy dots with different colors based on distance
            foreach (var enemyPos in enemyPositions)
            {
                var enemyMapPos = new Vector2(
                    miniMapPos.X + (enemyPos.X / worldBounds.Width) * miniMapSize.X,
                    miniMapPos.Y + (enemyPos.Y / worldBounds.Height) * miniMapSize.Y
                );
                
                float distanceToPlayer = Vector2.Distance(enemyPos, playerPos);
                Color enemyColor = distanceToPlayer < 100 ? Color.Red : distanceToPlayer < 200 ? Color.Orange : Color.Yellow;
                
                var enemyDot = new Rectangle((int)enemyMapPos.X - 1, (int)enemyMapPos.Y - 1, 3, 3);
                spriteBatch.Draw(_pixelTexture, enemyDot, enemyColor);
            }
            
            // Power-up dots
            foreach (var powerUpPos in powerUpPositions)
            {
                var powerUpMapPos = new Vector2(
                    miniMapPos.X + (powerUpPos.X / worldBounds.Width) * miniMapSize.X,
                    miniMapPos.Y + (powerUpPos.Y / worldBounds.Height) * miniMapSize.Y
                );
                
                var powerUpDot = new Rectangle((int)powerUpMapPos.X - 1, (int)powerUpMapPos.Y - 1, 3, 3);
                spriteBatch.Draw(_pixelTexture, powerUpDot, Color.Cyan);
            }
            
            // Mini-map title
            spriteBatch.DrawString(_font, "TACTICAL MAP", new Vector2(miniMapPos.X + 5, miniMapPos.Y - 25), Color.White);
            
            // Legend
            spriteBatch.DrawString(_font, "You", new Vector2(miniMapPos.X + 5, miniMapPos.Y + miniMapSize.Y + 5), Color.Lime);
            spriteBatch.DrawString(_font, "Enemies", new Vector2(miniMapPos.X + 50, miniMapPos.Y + miniMapSize.Y + 5), Color.Red);
            spriteBatch.DrawString(_font, "Items", new Vector2(miniMapPos.X + 120, miniMapPos.Y + miniMapSize.Y + 5), Color.Cyan);
        }

        public void DrawPerformanceMetrics(SpriteBatch spriteBatch, float fps, int bulletCount, int enemyCount, bool showAdvanced = false)
        {
            var viewport = _graphicsDevice.Viewport;
            var metricsPanel = new Rectangle(viewport.Width - 220, 10, 200, showAdvanced ? 120 : 80);
            
            // Background
            spriteBatch.Draw(_pixelTexture, metricsPanel, new Color(0, 0, 0, 180));
            DrawHUDPanel(spriteBatch, metricsPanel);
            
            // FPS with color coding
            Color fpsColor = fps >= 55 ? Color.Green : fps >= 30 ? Color.Yellow : Color.Red;
            spriteBatch.DrawString(_font, $"FPS: {fps:F0}", new Vector2(metricsPanel.X + 10, metricsPanel.Y + 10), fpsColor);
            
            // Entity counts
            spriteBatch.DrawString(_font, $"Bullets: {bulletCount}", new Vector2(metricsPanel.X + 10, metricsPanel.Y + 30), _counterColor);
            spriteBatch.DrawString(_font, $"Enemies: {enemyCount}", new Vector2(metricsPanel.X + 10, metricsPanel.Y + 50), _counterColor);
            
            if (showAdvanced)
            {
                // Performance bars
                var fpsBar = new Rectangle(metricsPanel.X + 10, metricsPanel.Y + 75, (int)(fps * 2), 8);
                var fpsBarBg = new Rectangle(metricsPanel.X + 10, metricsPanel.Y + 75, 120, 8);
                spriteBatch.Draw(_pixelTexture, fpsBarBg, Color.DarkGray);
                spriteBatch.Draw(_pixelTexture, fpsBar, fpsColor);
                
                spriteBatch.DrawString(_font, "Performance", new Vector2(metricsPanel.X + 10, metricsPanel.Y + 90), Color.Gray);
            }
        }

        public void DrawEnhancedHealthSystem(SpriteBatch spriteBatch, float currentHealth, float maxHealth, float shield = 0f, float maxShield = 0f)
        {
            var viewport = _graphicsDevice.Viewport;
            
            // Health bar (bottom center)
            var healthBarPos = new Vector2(viewport.Width/2 - 150, viewport.Height - 80);
            var healthBarSize = new Vector2(300, 20);
            
            // Background
            var healthBg = new Rectangle((int)healthBarPos.X, (int)healthBarPos.Y, (int)healthBarSize.X, (int)healthBarSize.Y);
            spriteBatch.Draw(_pixelTexture, healthBg, Color.DarkRed);
            
            // Health fill
            var healthFill = new Rectangle((int)healthBarPos.X, (int)healthBarPos.Y, 
                (int)(healthBarSize.X * (currentHealth / maxHealth)), (int)healthBarSize.Y);
            
            float healthPercentage = currentHealth / maxHealth;
            Color healthColor = healthPercentage > 0.6f ? Color.Green : 
                               healthPercentage > 0.3f ? Color.Yellow : Color.Red;
            spriteBatch.Draw(_pixelTexture, healthFill, healthColor);
            
            // Shield bar (if applicable)
            if (maxShield > 0)
            {
                var shieldBarPos = new Vector2(healthBarPos.X, healthBarPos.Y - 25);
                var shieldBg = new Rectangle((int)shieldBarPos.X, (int)shieldBarPos.Y, (int)healthBarSize.X, 15);
                var shieldFill = new Rectangle((int)shieldBarPos.X, (int)shieldBarPos.Y, 
                    (int)(healthBarSize.X * (shield / maxShield)), 15);
                
                spriteBatch.Draw(_pixelTexture, shieldBg, Color.DarkBlue);
                spriteBatch.Draw(_pixelTexture, shieldFill, Color.Cyan);
                DrawHUDPanel(spriteBatch, shieldBg);
                
                // Shield text
                spriteBatch.DrawString(_font, "SHIELD", new Vector2(shieldBarPos.X - 60, shieldBarPos.Y), Color.Cyan);
            }
            
            // Border and health text
            DrawHUDPanel(spriteBatch, healthBg);
            spriteBatch.DrawString(_font, "HEALTH", new Vector2(healthBarPos.X - 60, healthBarPos.Y), Color.White);
            
            // Health numbers
            string healthText = $"{currentHealth:F0}/{maxHealth:F0}";
            var healthTextSize = _font.MeasureString(healthText);
            spriteBatch.DrawString(_font, healthText, 
                new Vector2(healthBarPos.X + healthBarSize.X/2 - healthTextSize.X/2, healthBarPos.Y + 2), Color.White);
        }
    }
}
