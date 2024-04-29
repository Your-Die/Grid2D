using System;
using System.Collections.Generic;
using System.Linq;
using Chinchillada.Grid;
using JetBrains.Annotations;
using UnityEngine;

namespace Chinchillada.PCG.Grid
{
    [Serializable]
    public class CountingRule : ICellularRule
    {
        [SerializeField, UsedImplicitly] private string name;

        [SerializeField] private int output;

        [Header("Neighborhood")] [SerializeField]
        private int radius = 1;

        [SerializeReference] private INeighborhoodFactory neighborhoodFactory = new Diagonal.Factory();

        [SerializeField] private int constraintTarget = 0;

        [SerializeField] private CountConstraint constraint = new();

        public int Apply(int x, int y, Grid2D<int> grid)
        {
            int count = CountNeighborhood(x, y, grid, this.constraintTarget, this.radius, this.neighborhoodFactory);
            bool shouldApply = this.constraint.ValidateConstraint(count);

            return shouldApply ? this.output : grid[x, y];
        }

        public static int CountNeighborhood(int x,
                                            int y,
                                            Grid2D<int> grid,
                                            int targetValue,
                                            int radius,
                                            INeighborhoodFactory neighborhoodFactory)
        {
            GridNeighborhood neighborhood = neighborhoodFactory.Get(grid, x, y);

            return neighborhood.Select(neighbor => grid[neighbor])
                               .Count(neighbor => neighbor == targetValue);
        }
    }
}