using Chinchillada.Distributions;
using Chinchillada.Foundation;
using Chinchillada.Grid;
using UnityEngine;

namespace Chinchillada.Generation.Evolution.Grid
{
    public class GridMutator : Mutator<Grid2D>
    {
        [SerializeField] private float pixelMutationChance = 0.001f;

        [SerializeField, FindComponent] private IDistribution<int> valueDistribution;
        
        public override Grid2D Mutate(Grid2D parent, IRNG random)
        {
            var grid = parent.CopyShape();
            
            for (var x = 0; x < parent.Width; x++)
            for (var y = 0; y < parent.Height; y++)
            {
                var shouldMutate = random.Flip(this.pixelMutationChance);
                
                grid[x, y] = shouldMutate 
                ? this.valueDistribution.Sample()
                : parent[x, y];
            }

            return grid;
        }
    }
}