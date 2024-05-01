using System;
using System.Collections.Generic;
using System.Linq;
using Chinchillada.Grid;
using JetBrains.Annotations;
using UnityEngine;

namespace Chinchillada.PCG.Grid
{
    [Serializable]
    public class CountingRule<T> : ICellularRule<T>
    {
        [SerializeField, UsedImplicitly] private string name;

        [SerializeField] private T output;
        
        [SerializeField] private int targetValue = 0;

        [SerializeField] private CountConstraint constraint = new();

        [SerializeReference] private INeighborhoodFactory neighborhoodFactory = new Diagonal.Factory();

        public T Apply(int x, int y, Grid2D<T> grid)
        {
            int count = CountNeighborhood(x, y, grid, this.targetValue, this.neighborhoodFactory);
            bool shouldApply = this.constraint.ValidateConstraint(count);

            return shouldApply ? this.output : grid[x, y];
        }

        public static int CountNeighborhood(int x,
                                            int y,
                                            Grid2D<T> grid,
                                            int targetValue,
                                            INeighborhoodFactory neighborhoodFactory)
        {
            GridNeighborhood neighborhood = neighborhoodFactory.Get(grid, x, y);

            return neighborhood.Select(neighbor => grid[neighbor])
                               .Count(neighbor => neighbor.Equals(targetValue));
        }
    }
}