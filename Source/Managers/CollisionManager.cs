using System.Collections.Generic;
using MonoGame2DShooterPrototype.Source.Entities;
using MonoGame2DShooterPrototype.Source.Utilities;

namespace MonoGame2DShooterPrototype.Source.Managers
{
    public static class CollisionManager
    {
        public struct CollisionEvent
        {
            public object EntityA;
            public object EntityB;
            public string Type;
            public CollisionEvent(object a, object b, string type)
            {
                EntityA = a;
                EntityB = b;
                Type = type;
            }
        }

        // Generalized collision handler for two spatial grids
        public static List<CollisionEvent> HandleCollisions<TA, TB>(
            SpatialGrid<TA> gridA,
            SpatialGrid<TB> gridB,
            int gridCellSize,
            string collisionType = null)
            where TA : class
            where TB : class
        {
            var events = new List<CollisionEvent>();
            for (int ax = 0; ax < gridA.Cols; ax++)
            {
                for (int ay = 0; ay < gridA.Rows; ay++)
                {
                    var cellA = gridA.GetCell(ax, ay);
                    if (cellA == null) continue;
                    for (int ai = cellA.Count - 1; ai >= 0; ai--)
                    {
                        dynamic a = cellA[ai];
                        if (!a.IsActive) continue;
                        var boundsA = a.GetBounds();
                        int cellX = System.Math.Clamp(boundsA.X / gridCellSize, 0, gridB.Cols - 1);
                        int cellY = System.Math.Clamp(boundsA.Y / gridCellSize, 0, gridB.Rows - 1);

                        bool collided = false;
                        for (int dx = -1; dx <= 1 && !collided; dx++)
                        {
                            for (int dy = -1; dy <= 1 && !collided; dy++)
                            {
                                int nx = cellX + dx;
                                int ny = cellY + dy;
                                if (nx < 0 || nx >= gridB.Cols || ny < 0 || ny >= gridB.Rows) continue;
                                var cellB = gridB.GetCell(nx, ny);
                                if (cellB == null) continue;
                                for (int bi = cellB.Count - 1; bi >= 0; bi--)
                                {
                                    dynamic b = cellB[bi];
                                    if (a.IsActive && b.IsActive && a.GetBounds().Intersects(b.GetBounds()))
                                    {
                                        events.Add(new CollisionEvent(a, b, collisionType ?? $"{typeof(TA).Name}-{typeof(TB).Name}"));
                                        collided = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return events;
        }
    }
}
