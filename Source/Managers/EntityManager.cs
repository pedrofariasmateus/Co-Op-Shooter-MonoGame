// ...existing code...
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame2DShooterPrototype.Source.Entities;

namespace MonoGame2DShooterPrototype.Source.Managers
{
public class EntityManager
{
    private readonly List<IEntity> entities = new List<IEntity>();

    // Optional references for pooling
    public BulletManager BulletManager { get; set; }
    public EnemyManager EnemyManager { get; set; }

    /// <summary>
    /// Returns a read-only list of all managed entities.
    /// </summary>
    public IReadOnlyList<IEntity> GetEntities()
    {
        return entities.AsReadOnly();
    }

    public void RemoveInactive()
    {
        // Collect bullets and enemies to return to pool
        var toRemove = new List<IEntity>();
        foreach (var entity in entities)
        {
            bool isInactive = false;
            if (entity is MonoGame2DShooterPrototype.Source.Entities.Bullet bullet)
            {
                isInactive = !bullet.IsActive;
            }
            else if (entity is MonoGame2DShooterPrototype.Source.Entities.Enemy enemy)
            {
                isInactive = !enemy.IsActive;
            }
            else if (entity is MonoGame2DShooterPrototype.Source.Entities.Player player)
            {
                isInactive = !player.IsActive;
            }
            // Add more types with IsActive if needed

            if (isInactive)
            {
                if (entity is MonoGame2DShooterPrototype.Source.Entities.Bullet b && BulletManager != null)
                {
                    BulletManager.ReturnToPool(b);
                }
                if (entity is MonoGame2DShooterPrototype.Source.Entities.Enemy e && EnemyManager != null)
                {
                    EnemyManager.ReturnToPool(e);
                }
                toRemove.Add(entity);
            }
        }
        foreach (var entity in toRemove)
            entities.Remove(entity);
    }

    public void Register(IEntity entity)
    {
        if (entity != null && !entities.Contains(entity))
            entities.Add(entity);
    }

    public void RegisterRange(IEnumerable<IEntity> entityList)
    {
        foreach (var entity in entityList)
            Register(entity);
    }

    public void Update(GameTime gameTime)
    {
        // This method now requires explicit viewport size
        throw new System.NotImplementedException("Use Update(GameTime, int, int) instead.");
    }

    public void Update(GameTime gameTime, int viewportWidth, int viewportHeight)
    {
        foreach (var entity in entities)
            entity.Update(gameTime, viewportWidth, viewportHeight);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var entity in entities)
            entity.Draw(spriteBatch);
    }

    public void Clear()
    {
        entities.Clear();
    }
}
}
