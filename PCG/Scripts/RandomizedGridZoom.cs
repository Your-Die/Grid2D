using System.Collections.Generic;
using Chinchillada.Foundation;
using Chinchillada.Grid;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Chinchillada.Generation.Grid
{
    public class RandomizedGridZoom : AsyncGeneratorComponentBase<Grid2D>
    {
        [SerializeField] private int iterations;

        [SerializeField, FindComponent] private IAsyncGenerator<Grid2D> gridGenerator;

        [SerializeField] private IRNG random = new UnityRandom();

        private Grid2D grid;

        protected override Grid2D GenerateInternal()
        {
            this.GenerateGrid();

            for (var i = 0; i < this.iterations; i++)
                this.Zoom();

            return this.grid;
        }

        public override IEnumerable<Grid2D> GenerateAsync()
        {
            foreach (var result in this.gridGenerator.GenerateAsync())
            {
                this.grid = result;
                yield return this.grid;
            }

            for (var i = 0; i < this.iterations; i++)
            {
                this.Zoom();
                yield return this.grid;
            }
        }

        [Button]
        private void GenerateGrid()
        {
            this.grid = this.gridGenerator.Generate();
            this.OnGenerated(this.grid);
        }

        [Button]
        private void Zoom()
        {
            this.grid = RandomizedZoom(this.grid, this.random);
            this.OnGenerated(this.grid);
        }

        public static Grid2D RandomizedZoom(Grid2D grid, IRNG random)
        {
            var newWidth = grid.Width * 2 - 1;
            var newHeight = grid.Height * 2 - 1;

            var nextGrid = new Grid2D(newWidth, newHeight);

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
                nextGrid[newX       + 1, newY]     = random.Choose(topLeft, topRight);
                nextGrid[newX, newY + 1]           = random.Choose(topLeft, bottomLeft);
                nextGrid[newX       + 1, newY + 2] = random.Choose(bottomLeft, bottomRight);
                nextGrid[newX       + 2, newY + 1] = random.Choose(topRight, bottomRight);

                nextGrid[newX + 1, newY + 1] = random.Choose(topLeft, topRight, bottomLeft, bottomRight);
            }

            return nextGrid;
        }
    }
}