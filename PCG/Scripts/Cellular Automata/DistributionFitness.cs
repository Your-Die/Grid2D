using System;
using System.Linq;
using Chinchillada.Generation.Grid;
using Chinchillada.Foundation;
using Chinchillada.Grid;
using JetBrains.Annotations;

namespace Chinchillada.Generation.Evolution.Grid
{
    [UsedImplicitly]
    public class DistributionFitness : IMetricEvaluator<Grid2D>
    {
        public float Evaluate(Grid2D grid)
        {
            var dictionary = new DefaultDictionary<int, int>(0);
            
            for (var x = 0; x < grid.Width; x++)
            for (var y = 0; y < grid.Height; y++)
            {
                var value = grid[x, y];
                var sameNeighbors = CountingRule.CountNeighborhood(x, y, grid, value, 1);

                dictionary[value] += 1 + sameNeighbors;
            }

            var sum = dictionary.Values.Sum();
            var amount = dictionary.Keys.Count;

            return (float)Math.Pow(sum, amount);
        }
    }
}