using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using MonoGame2DShooterPrototype.Source.Managers;
using Microsoft.Xna.Framework.Input;
using MonoGame2DShooterPrototype.Source.Core;
using Entities = MonoGame2DShooterPrototype.Source.Entities;
using MonoGame2DShooterPrototype.Source.Entities;

namespace MonoGame2DShooterPrototype.Source.Screens
{
    // (Removed duplicate/corrupted GameScreen class definition)

    public class GameScreen : IGameScreen
    {
        // Shared score for both players
        private int score = 0;
        // Queue for batching enemy destroyed events
        private readonly List<(Entities.Enemy enemy, Entities.Bullet bullet)> enemyDestroyedQueue = new();
        // Spatial grid settings
        private const int GridCellSize = 64;
        private Utilities.SpatialGrid<Entities.Enemy> enemyGrid;
        private Utilities.SpatialGrid<Entities.Bullet> bulletGrid;

        private List<MonoGame2DShooterPrototype.Source.Entities.IEntity> players;
        private EntityManager entityManager;
        private Texture2D playerTexture;
        private Texture2D bulletTexture;
        private BulletManager bulletManager;
        private EnemyManager enemyManager;
        private Texture2D enemyTexture;
        private GameSettings settings;
        private GraphicsDevice graphicsDevice;
        private bool lastShootP1 = false;
        private bool lastShootP2 = false;
        private bool lastShootMouseP1 = false;
        private bool lastShootMouseP2 = false;
        private int poolSize = 100;
        private bool testModeActive = false;
        private bool lastTestKey = false;
        private SpriteFont debugFont;

        public GameScreen(GraphicsDevice graphicsDevice, GameSettings settings)
        {
            this.graphicsDevice = graphicsDevice;
            this.settings = settings;
        }

        public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            // Load debug font (ensure you have a SpriteFont asset named 'DefaultFont' in your Content project)
            debugFont = content.Load<SpriteFont>("DefaultFont");

            // Initialize grid
            enemyGrid = new Utilities.SpatialGrid<Entities.Enemy>(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height, GridCellSize);
            bulletGrid = new Utilities.SpatialGrid<Entities.Bullet>(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height, GridCellSize);

            // Load a simple player texture (replace with your own asset as needed)
            playerTexture = new Texture2D(graphicsDevice, 32, 32);
            Color[] data = new Color[32 * 32];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Cyan;
            playerTexture.SetData(data);

            // Use actual viewport size for player bounds
            int viewportWidth = graphicsDevice.Viewport.Width;
            int viewportHeight = graphicsDevice.Viewport.Height;

            // Bullet texture (simple 8x8 yellow square)
            bulletTexture = new Texture2D(graphicsDevice, 8, 8);
            Color[] bulletData = new Color[8 * 8];
            for (int i = 0; i < bulletData.Length; ++i) bulletData[i] = Color.Yellow;
            bulletTexture.SetData(bulletData);

            // Initialize bullet manager
            bulletManager = new BulletManager(bulletTexture, viewportWidth, viewportHeight, poolSize);

            // Create players and register them
            players = PlayerManager.CreatePlayers(playerTexture, settings, viewportWidth, viewportHeight);

            // Enemy texture (simple 24x24 red square)
            enemyTexture = new Texture2D(graphicsDevice, 24, 24);
            Color[] enemyData = new Color[24 * 24];
            for (int i = 0; i < enemyData.Length; ++i) enemyData[i] = Color.Red;
            enemyTexture.SetData(enemyData);
            // Initialize enemy manager
            enemyManager = new EnemyManager(enemyTexture, graphicsDevice);

            // Initialize entity manager and register all entities
            entityManager = new EntityManager();
            entityManager.BulletManager = bulletManager;
            entityManager.EnemyManager = enemyManager;
            entityManager.RegisterRange(players);
            // Enemies are now registered as they are spawned
            // Bullets are now registered as they are spawned
        }

        public void Update(GameTime gameTime)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            // --- Spatial grid update for enemies and bullets (optimized) ---
            var allEntities = entityManager.GetEntities();

            enemyGrid.Clear();
            foreach (var entity in allEntities)
            {
                if (entity != null && entity.GetType() == typeof(Entities.Enemy))
                {
                    var enemy = (Entities.Enemy)entity;
                    if (!enemy.IsActive) continue;
                    enemyGrid.Add(enemy, enemy.GetBounds());
                }
            }
            sw.Restart();
            bulletGrid.Clear();
            foreach (var entity in allEntities)
            {
                if (entity != null && entity.GetType() == typeof(Entities.Bullet))
                {
                    var bullet = (Entities.Bullet)entity;
                    if (!bullet.IsActive) continue;
                    bulletGrid.Add(bullet, bullet.GetBounds());
                }
            }
            sw.Restart();

            entityManager.Update(gameTime, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
            sw.Restart();

            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
#if DEBUG
            // Test mode: spawn 1000 bullets when T is pressed
            KeyboardState testKeyboardState = Keyboard.GetState();
            if (testKeyboardState.IsKeyDown(Keys.T))
            {
                if (!lastTestKey)
                {
                    testModeActive = true;
                    SpawnTestBullets(90);
                    lastTestKey = true;
                }
            }
            else
            {
                lastTestKey = false;
            }

            if (testKeyboardState.IsKeyDown(Keys.E))
            {
                if (!lastTestKey)
                {
                    testModeActive = true;
                    SpawnTestEnemies(100);
                    lastTestKey = true;
                }
            }
            else
            {
                lastTestKey = false;
            }
#endif

            // Player 1 shoot (keyboard)
            if (players.Count > 0)
            {
                var player1 = players[0] as Entities.Player;
                if (player1 != null)
                {
                    // Player 1 shoot (keyboard)
                    if (player1.InputSettings.KeyboardEnabled && keyboardState.IsKeyDown(GetKey(player1.InputSettings.KeyboardKeybindings.Shoot)))
                    {
                        if (!lastShootP1)
                        {
                            Vector2 dir1 = GetShootDirection(keyboardState, player1.InputSettings.KeyboardKeybindings);
                            Vector2 spawnOffset1 = GetBulletSpawnOffset(dir1, player1.Texture, bulletTexture);
                            var bullet = bulletManager.SpawnBullet(player1.Position + spawnOffset1, dir1);
                            entityManager.Register(bullet);
                            lastShootP1 = true;
                        }
                    }
                    else
                    {
                        lastShootP1 = false;
                    }

                    // Player 1 shoot (mouse)
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        if (!lastShootMouseP1)
                        {
                            Vector2 playerCenter = player1.Position + new Vector2(player1.Texture.Width / 2f, player1.Texture.Height / 2f);
                            Vector2 mousePos = new Vector2(mouseState.X, mouseState.Y);
                            Vector2 dirMouse = mousePos - playerCenter;
                            if (dirMouse != Vector2.Zero) dirMouse.Normalize();
                            Vector2 spawnOffsetMouse = GetBulletSpawnOffset(dirMouse, player1.Texture, bulletTexture);
                            var bullet = bulletManager.SpawnBullet(player1.Position + spawnOffsetMouse, dirMouse);
                            entityManager.Register(bullet);
                            lastShootMouseP1 = true;
                        }
                    }
                    else
                    {
                        lastShootMouseP1 = false;
                    }
                }
            }
            if (players.Count > 1)
            {
                var player2 = players[1] as Entities.Player;
                if (player2 != null)
                {
                    // Player 2 shoot (keyboard)
                    if (player2.InputSettings.KeyboardEnabled && keyboardState.IsKeyDown(GetKey(player2.InputSettings.KeyboardKeybindings.Shoot)))
                    {
                        if (!lastShootP2)
                        {
                            Vector2 dir2 = GetShootDirection(keyboardState, player2.InputSettings.KeyboardKeybindings);
                            Vector2 spawnOffset2 = GetBulletSpawnOffset(dir2, player2.Texture, bulletTexture);
                            var bullet = bulletManager.SpawnBullet(player2.Position + spawnOffset2, dir2);
                            entityManager.Register(bullet);
                            lastShootP2 = true;
                        }
                    }
                    else
                    {
                        lastShootP2 = false;
                    }

                    // Player 2 shoot (mouse)
                    if (mouseState.RightButton == ButtonState.Pressed)
                    {
                        if (!lastShootMouseP2)
                        {
                            Vector2 playerCenter2 = player2.Position + new Vector2(player2.Texture.Width / 2f, player2.Texture.Height / 2f);
                            Vector2 mousePos2 = new Vector2(mouseState.X, mouseState.Y);
                            Vector2 dirMouse2 = mousePos2 - playerCenter2;
                            if (dirMouse2 != Vector2.Zero) dirMouse2.Normalize();
                            Vector2 spawnOffsetMouse2 = GetBulletSpawnOffset(dirMouse2, player2.Texture, bulletTexture);
                            var bullet = bulletManager.SpawnBullet(player2.Position + spawnOffsetMouse2, dirMouse2);
                            entityManager.Register(bullet);
                            lastShootMouseP2 = true;
                        }
                    }
                    else
                    {
                        lastShootMouseP2 = false;
                    }
                }
            }

            // Generalized bullet-enemy collision using spatial grids
            var collisionEvents = CollisionManager.HandleCollisions<Entities.Bullet, Entities.Enemy>(
                bulletGrid, enemyGrid, GridCellSize, "Bullet-Enemy");
            sw.Restart();
            foreach (var evt in collisionEvents)
            {
                var bullet = evt.EntityA as Entities.Bullet;
                var enemy = evt.EntityB as Entities.Enemy;
                if (bullet != null) bullet.IsActive = false;
                if (enemy != null) enemy.IsActive = false;
                // Batch enemy destroyed events for later processing
                if (enemy != null && bullet != null)
                    enemyDestroyedQueue.Add((enemy, bullet));
            }
            sw.Restart();

            // Process a limited number of enemy destroyed events per frame
            const int maxExplosionsPerFrame = 10;
            int processed = 0;
            for (int i = 0; i < enemyDestroyedQueue.Count && processed < maxExplosionsPerFrame; i++, processed++)
            {
                var (enemy, bullet) = enemyDestroyedQueue[i];
                score += 100; // Example: 100 points per enemy destroyed
                GameEvents.RaiseEnemyDestroyed(enemy, bullet);
            }
            if (processed > 0)
                enemyDestroyedQueue.RemoveRange(0, processed);

            // Update and spawn enemies
            var newEnemy = enemyManager.UpdateAndMaybeSpawn(gameTime);
            if (newEnemy != null)
                entityManager.Register(newEnemy);

            // Remove inactive entities (bullets, enemies, etc.) every frame
            entityManager.RemoveInactive();
        }


        // Returns the direction vector based on movement keys pressed, defaults to up

        // Helper for key mapping

        // Helper to center bullet spawn based on direction

        // Test mode: spawn a burst of bullets for benchmarking

        // Test mode: spawn multiple enemies for collision testing

        public void Draw(SpriteBatch spriteBatch)
        {
            entityManager.Draw(spriteBatch);
#if DEBUG
            // Draw bullet/enemy counters for performance measurement
            int bulletCount = 0;
            int enemyCount = 0;
            foreach (var entity in entityManager.GetEntities())
            {
                if (entity is Entities.Bullet bullet && bullet.IsActive) bulletCount++;
                else if (entity is Entities.Enemy enemy && enemy.IsActive) enemyCount++;
            }
            // Draw counters and score in top-left (requires a SpriteFont called 'DefaultFont' in Content)
            if (debugFont != null)
            {
                string text = $"Score: {score}\nBullets: {bulletCount}\nEnemies: {enemyCount}";
                spriteBatch.DrawString(debugFont, text, new Vector2(10, 10), Color.Yellow);
            }
            // Optionally, draw test mode indicator
            if (testModeActive)
            {
                // Draw a simple red rectangle or text to indicate test mode (requires SpriteFont for text)
                // Example: spriteBatch.Draw(...)
                // For now, just leave as a comment
            }
#endif
        }

        // Returns the direction vector based on movement keys pressed, defaults to up
        private Vector2 GetShootDirection(KeyboardState keyboardState, KeyboardKeybindings keybindings)
        {
            Vector2 dir = Vector2.Zero;
            if (keyboardState.IsKeyDown(GetKey(keybindings.MoveUp))) dir.Y -= 1;
            if (keyboardState.IsKeyDown(GetKey(keybindings.MoveDown))) dir.Y += 1;
            if (keyboardState.IsKeyDown(GetKey(keybindings.MoveLeft))) dir.X -= 1;
            if (keyboardState.IsKeyDown(GetKey(keybindings.MoveRight))) dir.X += 1;
            if (dir == Vector2.Zero) dir = new Vector2(0, -1); // Default to up
            else dir.Normalize();
            return dir;
        }

        // Helper for key mapping
        private Keys GetKey(string keyName)
        {
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

        // Helper to center bullet spawn based on direction
        private Vector2 GetBulletSpawnOffset(Vector2 dir, Texture2D playerTex, Texture2D bulletTex)
        {
            float x = playerTex.Width / 2f - bulletTex.Width / 2f;
            float y = playerTex.Height / 2f - bulletTex.Height / 2f;
            // If shooting up or down, center horizontally
            if (dir.Y != 0 && dir.X == 0)
                return new Vector2(x, dir.Y < 0 ? 0 : playerTex.Height - bulletTex.Height);
            // If shooting left or right, center vertically
            if (dir.X != 0 && dir.Y == 0)
                return new Vector2(dir.X < 0 ? 0 : playerTex.Width - bulletTex.Width, y);
            // Diagonal: center both
            return new Vector2(x, y);
        }

        // Test mode: spawn a burst of bullets for benchmarking
        private void SpawnTestBullets(int count)
        {
            if (players.Count == 0 || players[0] == null) return;
            var player1 = players[0] as Entities.Player;
            if (player1 == null) return;
            float x = player1.Texture.Width / 2f - bulletTexture.Width / 2f;
            float y = player1.Texture.Height / 2f - bulletTexture.Height / 2f;
            Vector2 centeredOffset = new Vector2(x, y);
            for (int i = 0; i < count; i++)
            {
                // Spread bullets in a circle
                float angle = MathHelper.TwoPi * i / count;
                Vector2 dir = new Vector2(MathF.Cos(angle), MathF.Sin(angle));
                var bullet = bulletManager.SpawnBullet(player1.Position + centeredOffset, dir);
                entityManager.Register(bullet);
            }
        }

        // Test mode: spawn multiple enemies for collision testing
        private void SpawnTestEnemies(int count)
        {
            var newEnemies = enemyManager.SpawnTestEnemies(count);
            entityManager.RegisterRange(newEnemies);
        }
    }
}
