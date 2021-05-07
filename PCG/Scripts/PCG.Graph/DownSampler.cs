using Chinchillada;
using Chinchillada.Generation.Grid;
using Chinchillada.Grid;
using UnityEngine;

namespace Generators.Grid
{
    public class DownSampler : GridGeneratorNode
    {
        [SerializeField, Input] private Grid2D grid;

        [SerializeField, Input] private Vector2Int windowShape;

        [SerializeField] private IWindowSampler<int> windowSampler;

        protected override void UpdateInputs()
        {
            this.grid = this.GetInputValue(nameof(this.grid), this.grid);
            this.windowShape = this.GetInputValue(nameof(this.windowShape), this.windowShape);
        }

        protected override Grid2D GenerateGrid()
        {
            var shape = this.grid.Shape.DivideElementWise(this.windowShape);
            var output = new Grid2D(shape);

            var windowX = this.windowShape.x;
            var windowY = this.windowShape.y;

            var maxX = this.grid.Width - windowX;
            var maxY = this.grid.Height - windowY;

            for (var x = 0; x < maxX; x+= windowX)
            for (var y = 0; y < maxY; y += windowY)
            {
                var outputX = x / windowX;
                var outputY = y / windowY;

                output[outputX, outputY] = this.windowSampler.Sample(this.grid, x, y, this.windowShape);
            }

            return output;
        }
    }
}