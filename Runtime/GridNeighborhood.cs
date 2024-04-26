using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chinchillada.Grid
{
    public class GridNeighborhood : IEnumerable<(int, int)>, IEnumerable<Vector2Int>
    {
        public int CenterX { get; }
        public int CenterY { get; }
        public int Radius { get; }
       
        public int Top { get; }
        public int Left { get; }
        public int Right { get; }
        public int Bottom { get; }

        public GridNeighborhood(IGrid2D grid, Vector2Int center, int radius) : this(grid, center.x, center.y, radius)
        {
        }

        public GridNeighborhood(IGrid2D grid, int centerX, int centerY, int radius)
        {
            this.CenterX = centerX;
            this.CenterY = centerY;
            this.Radius = radius;

            this.Left = Mathf.Max(centerX - radius, 0);
            this.Top = Mathf.Max(centerY - radius, 0);
            this.Right = Mathf.Min(centerX + radius, grid.Width - 1);
            this.Bottom = Mathf.Min(centerY + radius, grid.Height - 1);
        }

        public IEnumerator<Vector2Int> GetEnumerator()
        {
            for (var x = this.Left; x <= this.Right; x++)
            for (var y = this.Top; y <= this.Bottom; y++)
                yield return new Vector2Int(x, y);
        }

        IEnumerator<(int, int)> IEnumerable<(int, int)>.GetEnumerator()
        {
            for (var x = this.Left; x <= this.Right; x++)
            for (var y = this.Top; y <= this.Bottom; y++)
                yield return (x, y);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}