using System.Collections.Generic;
using Chinchillada.Distributions;
using Chinchillada.Grid;
using UnityEngine;

namespace Chinchillada.Generation.Grid
{
    public class RandomGridGenerator : IterativeGeneratorComponent<Grid2D>
    {
        [SerializeField] private int width = 10;
        [SerializeField] private int height = 10;

        [SerializeField] private IDistribution<int> valueDistribution;

        public override IEnumerable<Grid2D> GenerateAsync()
        {
            yield return this.GenerateInternal();
        }

        protected override Grid2D GenerateInternal()
        {
            return GenerateGrid(this.width, this.height, this.valueDistribution);
        }

        public static Grid2D GenerateGrid(int width, int height, IDistribution<int> valueDistribution)
        {
            var items = new int[width, height];

            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                items[x, y] = valueDistribution.Sample();
            
            return new Grid2D(items);
        }
    }
}    