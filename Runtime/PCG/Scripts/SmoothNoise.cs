using Chinchillada.Grid;
using UnityEngine;

namespace Chinchillada.PCG.Grid
{
    public static class SmoothNoise 
    {
        public static Grid2D<int> Generate(Grid2D<int> grid, int samplePeriod)
        {
            var sampleFrequency = 1.0f / samplePeriod;
            var output = grid.CopyShape<int>();

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

                    output[x, y] = (int)Mathf.Lerp(top, bottom, verticalBlend);
                }
            }

            return output;
        }
    }
}