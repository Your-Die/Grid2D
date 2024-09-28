using System;
using UnityEngine;

namespace Chinchillada.Grid
{
    [Serializable]
    public abstract class NeighborhoodFactory<T> : INeighborhoodFactory
    {
        [SerializeField] private int  radius        = 1;
        [SerializeField] private bool includeCenter = false;

        private string Keyword => typeof(T).ToString();

        protected NeighborhoodFactory(int radius, bool includeCenter)
        {
            this.radius = radius;
            this.includeCenter = includeCenter;
        }

        public GridNeighborhood Get(IGrid2D grid, Vector2Int center)
        {
            return this.Get(grid, center.x, center.y);
        }

        public GridNeighborhood Get(IGrid2D grid, int centerX, int centerY)
        {
            return this.Get(grid, centerX, centerY, this.radius, this.includeCenter);
        }
        
        public override string ToString()
        {
            return $"{this.Keyword} {this.radius}";
        }
        
        protected abstract GridNeighborhood Get(IGrid2D grid, int centerX, int centerY, int radius, bool includeCenter);
    }
}