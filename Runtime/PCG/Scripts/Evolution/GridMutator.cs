using Chinchillada.Grid;
using UnityEngine;

namespace Chinchillada.PCG.Evolution.Grid
{
    public class GridMutator : Mutator<Grid2D<int>>
    {
        [SerializeField] private float pixelMutationChance = 0.001f;

        [SerializeField, FindComponent] private IDistribution<int> valueDistribution;

        public override Grid2D<int> Mutate(Grid2D<int> parent, IRNG random)
        {
            var grid = parent.CopyShape<int>();

            for (var x = 0; x < parent.Width; x++)
            for (var y = 0; y < parent.Height; y++)
            {
                var shouldMutate = random.Flip(this.pixelMutationChance);

                grid[x, y] = shouldMutate
                    ? this.valueDistribution.Sample(random)
                    : parent[x, y];
            }

            return grid;
        }
    }
}