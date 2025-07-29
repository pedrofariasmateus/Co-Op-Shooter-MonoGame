using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGame2DShooterPrototype.Source.Utilities
{
    public class SpatialGrid<T>
    {
        private readonly int cellSize;
        private readonly int cols;
        private readonly int rows;
        private readonly List<T>[,] grid;

        public int Cols => cols;
        public int Rows => rows;
        public int CellSize => cellSize;

        public SpatialGrid(int width, int height, int cellSize)
        {
            this.cellSize = cellSize;
            cols = (width + cellSize - 1) / cellSize;
            rows = (height + cellSize - 1) / cellSize;
            grid = new List<T>[cols, rows];
            for (int x = 0; x < cols; x++)
                for (int y = 0; y < rows; y++)
                    grid[x, y] = new List<T>();
        }

        public void Clear()
        {
            for (int x = 0; x < cols; x++)
                for (int y = 0; y < rows; y++)
                    grid[x, y].Clear();
        }

        public void Add(T item, Rectangle bounds)
        {
            int cellX = Math.Clamp(bounds.X / cellSize, 0, cols - 1);
            int cellY = Math.Clamp(bounds.Y / cellSize, 0, rows - 1);
            grid[cellX, cellY].Add(item);
        }

        public List<T> GetCell(int x, int y)
        {
            if (x < 0 || x >= cols || y < 0 || y >= rows) return null;
            return grid[x, y];
        }
    }
}
