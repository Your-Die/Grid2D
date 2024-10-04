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
            for (int y = this.Bottom; y <= this.Top; y++)
            {
                if (x != this.CenterX || y != this.CenterY || this.IncludeCenter)
                    yield return new Vector2Int(x, y);
            }
        }
        
        [Serializable]
        public class Factory : NeighborhoodFactory<Diagonal>
        {
            public Factory(int radius = 1, bool includeCenter = true) : base(radius, includeCenter)
            {
            }
            
            protected override GridNeighborhood Get(IGrid2D grid, int centerX, int centerY, int radius, bool includeCenter)
            {
                return new Diagonal(grid, centerX, centerY, radius, includeCenter);
            }
        }
    }
}