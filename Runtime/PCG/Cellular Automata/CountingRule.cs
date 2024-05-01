using System;
using System.Linq;
using Chinchillada.Grid;
using UnityEngine;

namespace Chinchillada.PCG.Grid
{
    [Serializable]
    public class CountingRule<T> : ICellularRule<T>
    {
        [SerializeField] private T output;
        
        [SerializeField] private int targetValue = 0;

        [SerializeField] private CountConstraint constraint = new();

        [SerializeReference] private INeighborhoodFactory neighborhoodFactory = new Diagonal.Factory();

        public CountingRule()
        {
        }

        public CountingRule(T output, int targetValue, CountConstraint constraint, INeighborhoodFactory neighborhood)
        {
            this.output = output;
            this.targetValue = targetValue;
            this.constraint = constraint;
            this.neighborhoodFactory = neighborhood;
        }

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

        public override string ToString()
        {
            return $"count({this.targetValue}) {this.constraint} in [{this.neighborhoodFactory}] -> {this.output}";
        }
    }
}