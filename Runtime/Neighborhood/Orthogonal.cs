using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chinchillada.Grid
{
    public class Orthogonal : GridNeighborhood
    {
        private Orthogonal(IGrid2D grid, int centerX, int centerY, int radius, bool includeCenter)
            : base(grid, centerX, centerY, radius, includeCenter)
        {
        }

        public override IEnumerator<Vector2Int> GetEnumerator()
        {
            if (this.IncludeCenter)
                yield return new Vector2Int(this.CenterX, this.CenterY);

            for (int y = this.Bottom; y < this.CenterY; y++)
                yield return new Vector2Int(this.CenterX, y);

            for (int y = this.CenterY + 1; y <= this.Top; y++)
                yield return new Vector2Int(this.CenterX, y);

            for (int x = this.Left; x < this.CenterX; x++)
                yield return new Vector2Int(x, this.CenterY);

            for (int x = this.CenterX + 1; x <= this.Right; x++)
                yield return new Vector2Int(x, this.CenterY);
        }

        [Serializable]
        public class Factory : NeighborhoodFactory
        {
            protected override GridNeighborhood Get(IGrid2D grid, int centerX, int centerY, int radius, bool includeCenter)
            {
                return new Orthogonal(grid, centerX, centerY, radius, includeCenter);
            }
        }
    }
}