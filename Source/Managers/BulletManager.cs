using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame2DShooterPrototype.Source.Entities;

namespace MonoGame2DShooterPrototype.Source.Managers
{
public class BulletManager
{
    private readonly Queue<Bullet> bulletPool = new Queue<Bullet>();
    private readonly int poolSize;
    private Texture2D bulletTexture;
    private int screenWidth;
    private int screenHeight;

    public BulletManager(Texture2D bulletTexture, int screenWidth, int screenHeight, int poolSize = 100)
    {
        this.bulletTexture = bulletTexture;
        this.screenWidth = screenWidth;
        this.screenHeight = screenHeight;
        this.poolSize = poolSize;

        for (int i = 0; i < poolSize; i++)
        {
            bulletPool.Enqueue(new Bullet(bulletTexture, Vector2.Zero, Vector2.Zero, screenWidth, screenHeight));
        }
    }

    /// <summary>
    /// Spawns a bullet and returns it for registration with EntityManager.
    /// </summary>
    public IEntity SpawnBullet(Vector2 position, Vector2 direction)
    {
        Bullet bullet;
        if (bulletPool.Count > 0)
        {
            bullet = bulletPool.Dequeue();
            bullet.Texture = bulletTexture;
            bullet.Position = position;
            bullet.Direction = direction;
            bullet.IsActive = true;
        }
        else
        {
            bullet = new Bullet(bulletTexture, position, direction, screenWidth, screenHeight);
        }
        return bullet;
    }

    /// <summary>
    /// Returns a bullet to the pool for reuse.
    /// Call this from EntityManager when a bullet becomes inactive.
    /// </summary>
    public void ReturnToPool(Bullet bullet)
    {
        if (bullet != null)
        {
            bullet.IsActive = false;
            bulletPool.Enqueue(bullet);
        }
    }
}
}
