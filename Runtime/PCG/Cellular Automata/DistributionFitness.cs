using System;
using System.Linq;
using Chinchillada;
using Chinchillada.Grid;
using Chinchillada.PCG.Grid;
using JetBrains.Annotations;
using UnityEngine;

namespace Chinchillada.PCG.Evolution.Grid
{
    [UsedImplicitly, Serializable]
    public class DistributionFitness : IMetricEvaluator<Grid2D<int>>
    {
        [SerializeReference] private INeighborhoodFactory neighborhoodFactory = new Diagonal.Factory();
        
        public float Evaluate(Grid2D<int> grid)
        {
            var dictionary = new DefaultDictionary<int, int>(0);
            
            for (int x = 0; x < grid.Width; x++)
            for (int y = 0; y < grid.Height; y++)
            {
                int value = grid[x, y];
                int sameNeighbors = CountingRule<int>.CountNeighborhood(x, y, grid, value, this.neighborhoodFactory);

                dictionary[value] += 1 + sameNeighbors;
            }

            int sum = dictionary.Values.Sum();
            int amount = dictionary.Keys.Count;

            return (float)Math.Pow(sum, amount);
        }
    }
}