using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGame2DShooterPrototype.Source.Core
{
    public enum GamePlayState
    {
        Playing,
        Paused,
        GameOver
    }

    public class GameStateManager
    {
        public GamePlayState CurrentState { get; private set; } = GamePlayState.Playing;
        public int CurrentWave { get; set; } = 1;
        public float WaveTimeRemaining { get; set; } = 0f;
        public int ComboCount { get; set; } = 0;
        public float ComboTimer { get; set; } = 0f;
        public float PlayerHealth { get; set; } = 100f;
        public string CurrentWeapon { get; set; } = "Basic Blaster";
        public int CurrentAmmo { get; set; } = -1; // -1 for unlimited
        public float CurrentFireRate { get; set; } = 0.5f;
        public string ActivePowerUp { get; set; } = "";
        public float PowerUpTimeRemaining { get; set; } = 0f;

        private bool _lastPauseKey = false;
        private bool _lastRestartKey = false;

        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Handle pause/unpause
            bool currentPauseKey = keyboardState.IsKeyDown(Keys.P);
            if (currentPauseKey && !_lastPauseKey)
            {
                if (CurrentState == GamePlayState.Playing)
                    CurrentState = GamePlayState.Paused;
                else if (CurrentState == GamePlayState.Paused)
                    CurrentState = GamePlayState.Playing;
            }
            _lastPauseKey = currentPauseKey;

            // Handle restart
            bool currentRestartKey = keyboardState.IsKeyDown(Keys.R);
            if (currentRestartKey && !_lastRestartKey && CurrentState == GamePlayState.GameOver)
            {
                RestartGame();
            }
            _lastRestartKey = currentRestartKey;

            // Only update game logic when playing
            if (CurrentState == GamePlayState.Playing)
            {
                // Update combo timer
                if (ComboTimer > 0)
                {
                    ComboTimer -= deltaTime;
                    if (ComboTimer <= 0)
                    {
                        ComboCount = 0;
                    }
                }

                // Update power-up timer
                if (PowerUpTimeRemaining > 0)
                {
                    PowerUpTimeRemaining -= deltaTime;
                    if (PowerUpTimeRemaining <= 0)
                    {
                        ActivePowerUp = "";
                    }
                }

                // Update wave timer
                if (WaveTimeRemaining > 0)
                {
                    WaveTimeRemaining -= deltaTime;
                }

                // Check for game over
                if (PlayerHealth <= 0)
                {
                    CurrentState = GamePlayState.GameOver;
                }
            }
        }

        public void AddCombo()
        {
            ComboCount++;
            ComboTimer = 3.0f; // Reset combo timer to 3 seconds
        }

        public void TakeDamage(float damage)
        {
            PlayerHealth = MathHelper.Max(0, PlayerHealth - damage);
        }

        public void HealPlayer(float healing)
        {
            PlayerHealth = MathHelper.Min(100f, PlayerHealth + healing);
        }

        public void ActivatePowerUp(string powerUpName, float duration)
        {
            ActivePowerUp = powerUpName;
            PowerUpTimeRemaining = duration;
        }

        public void StartNewWave(int waveNumber, float waveDuration = 60f)
        {
            CurrentWave = waveNumber;
            WaveTimeRemaining = waveDuration;
        }

        public void RestartGame()
        {
            CurrentState = GamePlayState.Playing;
            CurrentWave = 1;
            WaveTimeRemaining = 60f;
            ComboCount = 0;
            ComboTimer = 0f;
            PlayerHealth = 100f;
            CurrentWeapon = "Basic Blaster";
            CurrentAmmo = -1;
            CurrentFireRate = 0.5f;
            ActivePowerUp = "";
            PowerUpTimeRemaining = 0f;
        }

        public int GetScoreMultiplier()
        {
            return MathHelper.Max(1, ComboCount / 3); // Every 3 combo hits increases multiplier
        }
    }
}
