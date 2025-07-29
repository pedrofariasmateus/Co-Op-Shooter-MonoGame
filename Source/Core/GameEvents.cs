using System;
using MonoGame2DShooterPrototype.Source.Entities;

namespace MonoGame2DShooterPrototype.Source.Core
{
    public static class GameEvents
    {
        // Raised when an enemy is destroyed (e.g., by a bullet)
        public static event Action<MonoGame2DShooterPrototype.Source.Entities.Enemy, MonoGame2DShooterPrototype.Source.Entities.Bullet> OnEnemyDestroyed;
        // Raised when a bullet hits any target
        public static event Action<MonoGame2DShooterPrototype.Source.Entities.Bullet, object> OnBulletHit;

        public static void RaiseEnemyDestroyed(MonoGame2DShooterPrototype.Source.Entities.Enemy enemy, MonoGame2DShooterPrototype.Source.Entities.Bullet bullet)
        {
            OnEnemyDestroyed?.Invoke(enemy, bullet);
        }

        public static void RaiseBulletHit(MonoGame2DShooterPrototype.Source.Entities.Bullet bullet, object target)
        {
            OnBulletHit?.Invoke(bullet, target);
        }
    }
}
