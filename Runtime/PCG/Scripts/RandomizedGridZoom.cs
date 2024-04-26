using System.Collections.Generic;
using Chinchillada.Grid;

namespace Chinchillada.PCG.Grid
{
    public class RandomizedGridZoom : AsyncGeneratorBase<Grid2D<int>>
    {
        private readonly Grid2D<int> inputGrid;

        private readonly int iterations;

        public RandomizedGridZoom(Grid2D<int> grid, int iterations)
        {
            this.inputGrid = grid;
            this.iterations = iterations;
        }

        public override IEnumerable<Grid2D<int>> GenerateAsync(IRNG random)
        {
            var grid = this.inputGrid;

            for (var i = 0; i < this.iterations; i++)
            {
                grid = RandomizedZoom(grid, random);
                yield return grid;
            }
        }

        public static Grid2D<int> RandomizedZoom(Grid2D<int> grid, IRNG random)
        {
            var newWidth = grid.Width * 2 - 1;
            var newHeight = grid.Height * 2 - 1;

            var nextGrid = new Grid2D<int>(newWidth, newHeight);

            // sliding 2x2 window.
            for (var x = 0; x < grid.Width - 1; x++)
            for (var y = 0; y < grid.Height - 1; y++)
            {
                var topLeft = grid[x, y];
                var topRight = grid[x + 1, y];
                var bottomLeft = grid[x, y + 1];
                var bottomRight = grid[x + 1, y + 1];

                // window is used to fill in 3x3 window in next grid.
                var newX = x * 2;
                var newY = y * 2;

                // Copy corners.
                nextGrid[newX, newY] = topLeft;
                nextGrid[newX + 2, newY] = topRight;
                nextGrid[newX, newY + 2] = bottomLeft;
                nextGrid[newX + 2, newY + 2] = bottomRight;

                // Choose sides
                nextGrid[newX + 1, newY] = random.Choose(topLeft, topRight);
                nextGrid[newX, newY + 1] = random.Choose(topLeft, bottomLeft);
                nextGrid[newX + 1, newY + 2] = random.Choose(bottomLeft, bottomRight);
                nextGrid[newX + 2, newY + 1] = random.Choose(topRight, bottomRight);

                nextGrid[newX + 1, newY + 1] = random.Choose(topLeft, topRight, bottomLeft, bottomRight);
            }

            return nextGrid;
        }
    }
}