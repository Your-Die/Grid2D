using System;
using System.Collections.Generic;
using System.Linq;
using Chinchillada.Grid;
using UnityEngine;

namespace Chinchillada.PCG.Grid
{
    [Serializable]
    public class CountingRule : ICellularRule
    {
        [SerializeField] private string name;

        [SerializeField] private int output;

        [Header("Neighborhood")]
        [SerializeField]
        private int radius = 1;

        [SerializeField] private NeighborhoodType neighborhoodType;

        [SerializeField] private int constraintTarget = 0;

        [SerializeField] private CountConstraint constraint = new CountConstraint();

        public int Apply(int x, int y, Grid2D<int> grid)
        {
            var count       = CountNeighborhood(x, y, grid, this.constraintTarget, this.radius, this.neighborhoodType);
            var shouldApply = this.constraint.ValidateConstraint(count);

            return shouldApply ? this.output : grid[x, y];
        }

        public static int CountNeighborhood(int              x,
                                            int              y,
                                            Grid2D<int> grid,
                                            int              targetValue,
                                            int              radius,
                                            NeighborhoodType neighborhoodType = NeighborhoodType.Full)
        {
            IEnumerable<int> neighbors;
            switch (neighborhoodType)
            {
                case NeighborhoodType.Orthogonal:
                    neighbors = GetOrthogonalNeighbors(x, y, grid, radius);
                    break;
                case NeighborhoodType.Full:
                    neighbors = GetAllNeighbors(x, y, grid, radius);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return neighbors.Count(neighbor => neighbor == targetValue);
        }

        /// <summary>
        /// Get the orthogonally connected neighbors around (<paramref name="x"/>, <paramref name="y"/>)
        /// on the <paramref name="grid"/>.
        /// </summary>
        private static IEnumerable<int> GetOrthogonalNeighbors(int centerX, int centerY, Grid2D<int> grid, int radius)
        {
            var region = grid.GetRegion(centerX, centerY, radius);

            for (var x = region.Left; x < centerX; x++)
                yield return grid[x, centerY];

            for (var x = centerX + 1; x <= region.Right; x++)
                yield return grid[x, centerY];

            for (var y = region.Top; y < centerY; y++)
                yield return grid[centerX, y];

            for (var y = centerY + 1; y <= region.Bottom; y++)
                yield return grid[centerX, y];
        }

        /// <summary>
        /// Get all neighbors around (<paramref name="x"/>, <paramref name="y"/>) on the <paramref name="grid"/>.
        /// </summary>
        private static IEnumerable<int> GetAllNeighbors(int centerX, int centerY, Grid2D<int> grid, int radius)
        {
            var region = grid.GetRegion(centerX, centerY, radius);

            foreach (var cell in region)
            {
                if (cell.x != centerX || cell.y != centerY)
                    yield return grid[cell];
            }
        }
    }
}