using Chinchillada.Generation.Grid;
using Chinchillada.Grid;
using UnityEngine;

namespace Generators.Grid
{
    using System.Runtime.CompilerServices;
    using Chinchillada;

    public class RandomizedZoomNode : GridGeneratorNode
    {
        [SerializeField, Input] private Grid2D grid;
        
        [SerializeField, Input] private int iterations = 1;
        
        protected override void UpdateInputs()
        {
            this.grid = this.GetInputValue(nameof(this.grid), this.grid);
            this.iterations = this.GetInputValue(nameof(this.iterations), this.iterations);
        }

        protected override Grid2D GenerateGrid() => PerformIterations(this.grid, this.iterations, this.Random);

        private static Grid2D PerformIterations(Grid2D grid, int iterations, IRNG random)
        {
            for (var i = 0; i < iterations; i++) 
                grid = RandomizedGridZoom.RandomizedZoom(grid, random);

            return grid;
        }
    }
}