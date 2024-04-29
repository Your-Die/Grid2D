using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chinchillada.Grid
{
    public abstract class GridNeighborhood : IEnumerable<Vector2Int>
    {
        protected bool IncludeCenter { get; }

        public int CenterX { get; }
        public int CenterY { get; }
        public int Radius  { get; }

        public int Top    { get; }
        public int Left   { get; }
        public int Right  { get; }
        public int Bottom { get; }

        protected GridNeighborhood(IGrid2D grid, int centerX, int centerY, int radius, bool includeCenter)
        {
            this.CenterX = centerX;
            this.CenterY = centerY;
            this.Radius = radius;
            this.IncludeCenter = includeCenter;

            this.Left = Mathf.Max(centerX - radius, 0);
            this.Top = Mathf.Max(centerY - radius, 0);
            this.Right = Mathf.Min(centerX + radius, grid.Width - 1);
            this.Bottom = Mathf.Min(centerY + radius, grid.Height - 1);
        }

        public abstract IEnumerator<Vector2Int> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}