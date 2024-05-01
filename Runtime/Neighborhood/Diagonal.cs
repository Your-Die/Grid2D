using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chinchillada.Grid
{
    public class Diagonal : GridNeighborhood
    {
        private Diagonal(IGrid2D grid, int centerX, int centerY, int radius, bool includeCenter = false)
            : base(grid, centerX, centerY, radius, includeCenter)
        {
        }

        public override IEnumerator<Vector2Int> GetEnumerator()
        {
            for (int x = this.Left; x <= this.Right; x++)
            for (int y = this.Top; y <= this.Bottom; y++)
            {
                if (x != this.CenterX || y != this.CenterY || this.IncludeCenter)
                    yield return new Vector2Int(x, y);
            }
        }
        
        [Serializable]
        public class Factory : NeighborhoodFactory<Diagonal>
        {
            protected override GridNeighborhood Get(IGrid2D grid, int centerX, int centerY, int radius, bool includeCenter)
            {
                return new Diagonal(grid, centerX, centerY, radius, includeCenter);
            }
        }
    }
}