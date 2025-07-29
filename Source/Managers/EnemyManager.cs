using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame2DShooterPrototype.Source.Entities;

namespace MonoGame2DShooterPrototype.Source.Managers
{
public class EnemyManager
{
    private readonly Texture2D enemyTexture;
    private readonly GraphicsDevice graphicsDevice;
    private readonly int viewportWidth;
    private readonly int viewportHeight;
    private readonly Random random = new Random();
    private double spawnTimer = 0;
    private double spawnInterval = 2.0; // seconds
    private readonly Queue<Enemy> enemyPool = new Queue<Enemy>();
    private readonly int poolSize = 100;

    public EnemyManager(Texture2D enemyTexture, GraphicsDevice graphicsDevice)
    {
        this.enemyTexture = enemyTexture;
        this.graphicsDevice = graphicsDevice;
        this.viewportWidth = graphicsDevice.Viewport.Width;
        this.viewportHeight = graphicsDevice.Viewport.Height;

        // Pre-populate pool
        for (int i = 0; i < poolSize; i++)
        {
            enemyPool.Enqueue(new Enemy(enemyTexture, Vector2.Zero, Vector2.Zero));
        }
    }

    /// <summary>
    /// Call this each frame to determine if a new enemy should be spawned. Returns a new enemy if spawned, otherwise null.
    /// </summary>
    public IEntity UpdateAndMaybeSpawn(GameTime gameTime)
    {
        spawnTimer += gameTime.ElapsedGameTime.TotalSeconds;
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0;
            return SpawnEnemy();
        }
        return null;
    }

    /// <summary>
    /// Spawns a single enemy and returns it for registration with EntityManager.
    /// </summary>
    public IEntity SpawnEnemy()
    {
        int x = random.Next(0, viewportWidth - enemyTexture.Width);
        Vector2 position = new Vector2(x, -enemyTexture.Height);
        Vector2 velocity = new Vector2(0, 60); // Move down at 60 px/sec
        Enemy enemy;
        if (enemyPool.Count > 0)
        {
            enemy = enemyPool.Dequeue();
            enemy.Texture = enemyTexture;
            enemy.Position = position;
            enemy.Velocity = velocity;
            enemy.IsActive = true;
        }
        else
        {
            enemy = new Enemy(enemyTexture, position, velocity);
        }
        return enemy;
    }

    /// <summary>
    /// For test mode: spawn multiple enemies and return them for registration.
    /// </summary>
    public List<IEntity> SpawnTestEnemies(int count)
    {
        var newEnemies = new List<IEntity>();
        for (int i = 0; i < count; i++)
        {
            int x = random.Next(0, viewportWidth - enemyTexture.Width);
            Vector2 position = new Vector2(x, -enemyTexture.Height);
            Vector2 velocity = new Vector2(0, 60);
            Enemy enemy;
            if (enemyPool.Count > 0)
            {
                enemy = enemyPool.Dequeue();
                enemy.Texture = enemyTexture;
                enemy.Position = position;
                enemy.Velocity = velocity;
                enemy.IsActive = true;
            }
            else
            {
                enemy = new Enemy(enemyTexture, position, velocity);
            }
            newEnemies.Add(enemy);
        }
        return newEnemies;
    }

    /// <summary>
    /// Returns an enemy to the pool for reuse.
    /// </summary>
    public void ReturnToPool(Enemy enemy)
    {
        if (enemy != null)
        {
            enemy.IsActive = false;
            enemyPool.Enqueue(enemy);
        }
    }
}
}
