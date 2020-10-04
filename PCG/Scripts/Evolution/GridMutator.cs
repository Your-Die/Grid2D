using Chinchillada.Distributions;
using Chinchillada.Foundation;
using Chinchillada.Grid;
using UnityEngine;
using Random = Chinchillada.Foundation.Random;

namespace Chinchillada.Generation.Evolution.Grid
{
    public class GridMutator : Mutator<Grid2D>
    {
        [SerializeField] private float pixelMutationChance = 0.001f;

        [SerializeField, FindComponent] private IDistribution<int> valueDistribution;
        
        public override Grid2D Mutate(Grid2D parent)
        {
            var grid = parent.CopyShape();
            
            for (var x = 0; x < parent.Width; x++)
            for (var y = 0; y < parent.Height; y++)
            {
                var shouldMutate = Random.Bool(this.pixelMutationChance);
                
                grid[x, y] = shouldMutate 
                ? this.valueDistribution.Sample()
                : parent[x, y];
            }

            return grid;
        }
    }
}