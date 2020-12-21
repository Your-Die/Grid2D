using Chinchillada.CellularAutomata;
using Chinchillada.Generation.Grid;
using Chinchillada.Grid;
using UnityEngine;

namespace Generators.Grid
{
    public class CA1HeightDownSampler : GridGeneratorNode
    {
        [SerializeField, Input] private Grid2D grid;

        [SerializeField] private CellularAutomata1D<int> automaton;
        
        protected override void UpdateInputs()
        {
            this.grid = this.GetInputValue(nameof(this.grid), this.grid);
        }

        protected override Grid2D GenerateGrid()
        {
            var height = this.grid.Height / 3;
            var width = this.grid.Width;
            
            var output = new Grid2D(width, height);

            var inputColumn = new GridColumnAccessor(this.grid);
            var outputColumn = new GridColumnAccessor(output);

            for (var x = 0; x < width; x++)
            {
                inputColumn.ColumnIndex = x;
                outputColumn.ColumnIndex = x;

                for (var y = 1; y < height; y += 3)
                {
                    var rule = this.automaton.FindMatchingRule(inputColumn, y);

                    var outputY = y / 3;
                    rule.Apply(outputY, outputColumn);
                }
            }

            return output;
        }
    }
}