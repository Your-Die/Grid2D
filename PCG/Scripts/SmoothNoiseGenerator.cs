using System.Collections.Generic;
using Chinchillada;
using Chinchillada.Grid;
using UnityEngine;

namespace Chinchillada.Generation.Grid
{
    public class SmoothNoiseGenerator : AsyncGeneratorComponentBase<Grid2D>
    {
        [SerializeField] private int samplePeriod = 1;

        [SerializeField] private IAsyncGenerator<Grid2D> gridGenerator;

        public override IEnumerable<Grid2D> GenerateAsync()
        {
            foreach (var result in this.gridGenerator.GenerateAsync())
                yield return result;

            yield return SmoothNoise(this.Result, this.samplePeriod);
        }

        protected override Grid2D GenerateInternal()
        {
            var grid = this.gridGenerator.Generate();
            return SmoothNoise(grid, this.samplePeriod);
        }

        public static Grid2D SmoothNoise(Grid2D grid, int samplePeriod)
        {
            var sampleFrequency = 1.0f / samplePeriod;
            var output = grid.CopyShape();

            for (var x = 0; x < output.Width; x++)
            {
                var leftBorder = x.ClosestSmallerMultiple(samplePeriod);
                var rightBorder = (leftBorder + samplePeriod) % output.Width;

                var horizontalBlend = (x - leftBorder) * sampleFrequency;

                for (var y = 0; y < output.Height; y++)
                {
                    var topBorder = y.ClosestSmallerMultiple(samplePeriod);
                    var bottomBorder = (topBorder + samplePeriod) % output.Height;

                    var verticalBlend = (y - topBorder) * sampleFrequency;

                    var topLeft = grid[leftBorder, topBorder];
                    var topRight = grid[rightBorder, topBorder];
                    var bottomLeft = grid[leftBorder, bottomBorder];
                    var bottomRight = grid[rightBorder, bottomBorder];

                    var top = Mathf.Lerp(topLeft, topRight, horizontalBlend);
                    var bottom = Mathf.Lerp(bottomLeft, bottomRight, horizontalBlend);

                    output[x, y] = (int) Mathf.Lerp(top, bottom, verticalBlend);
                }
            }

            return output;
        }
    }
}