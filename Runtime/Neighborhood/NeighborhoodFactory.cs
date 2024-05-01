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

        public GridNeighborhood Get(IGrid2D grid, Vector2Int center) => this.Get(grid, center.x, center.y);

        public GridNeighborhood Get(IGrid2D grid, int centerX, int centerY)
        {
            return this.Get(grid, centerX, centerY, this.radius, this.includeCenter);
        }

        protected abstract GridNeighborhood Get(IGrid2D grid, int centerX, int centerY, int radius, bool includeCenter);

        public override string ToString() => $"{this.Keyword} {this.radius}";
    }
}