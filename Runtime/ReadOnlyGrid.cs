using System;
using UnityEngine;

namespace Chinchillada.Grid
{
    public class ReadOnlyGrid<T> : IGrid2D<T>
    {
        private readonly IGrid2D<T> grid;

        public ReadOnlyGrid(IGrid2D<T> grid)
        {
            this.grid = grid;
        }

        public int Height => this.grid.Height;

        public int Width
        {
            get => this.grid.Width;
            set => throw new InvalidOperationException("Not allowed on Read-only grid");
        }

        public T this[int x, int y]
        {
            get => this.grid[x, y];
            set => throw new InvalidOperationException("Not allowed on Read-only grid");
        }

        public T this[Vector2Int position]
        {
            get => this.grid[position];
            set => throw new InvalidOperationException("Not allowed on Read-only grid");
        }
    }
}