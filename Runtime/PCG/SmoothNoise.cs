using Chinchillada.Grid;
using UnityEngine;

namespace Chinchillada.PCG.Grid
{
    public static class SmoothNoise 
    {
        public static Grid2D<int> Generate(Grid2D<int> grid, int samplePeriod)
        {
            float sampleFrequency = 1.0f / samplePeriod;
            var output = grid.CopyShape<int>();

            for (int x = 0; x < output.Width; x++)
            {
                int leftBorder = x.ClosestSmallerMultiple(samplePeriod);
                int rightBorder = (leftBorder + samplePeriod) % output.Width;

                float horizontalBlend = (x - leftBorder) * sampleFrequency;

                for (int y = 0; y < output.Height; y++)
                {
                    int topBorder = y.ClosestSmallerMultiple(samplePeriod);
                    int bottomBorder = (topBorder + samplePeriod) % output.Height;

                    float verticalBlend = (y - topBorder) * sampleFrequency;

                    int topLeft = grid[leftBorder, topBorder];
                    int topRight = grid[rightBorder, topBorder];
                    int bottomLeft = grid[leftBorder, bottomBorder];
                    int bottomRight = grid[rightBorder, bottomBorder];

                    float top = Mathf.Lerp(topLeft, topRight, horizontalBlend);
                    float bottom = Mathf.Lerp(bottomLeft, bottomRight, horizontalBlend);

                    output[x, y] = (int)Mathf.Lerp(top, bottom, verticalBlend);
                }
            }

            return output;
        }
    }
}