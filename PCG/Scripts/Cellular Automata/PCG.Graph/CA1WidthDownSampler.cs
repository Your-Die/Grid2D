using Chinchillada.CellularAutomata;
using Chinchillada.Generation.Grid;
using Chinchillada.Grid;
using UnityEngine;

namespace Generators.Grid
{
    public class CA1WidthDownSampler : GridGeneratorNode
    {
        [SerializeField, Input] private Grid2D grid;

        [SerializeField] private CellularAutomata1D<int> automaton;
        
        protected override void UpdateInputs()
        {
            this.grid = this.GetInputValue(nameof(this.grid), this.grid);
        }

        protected override Grid2D GenerateGrid()
        {
            var width = this.grid.Width / 3;
            var height = this.grid.Height;
            
            var output = new Grid2D(width, height);

            var inputRow = new GridRowAccessor(this.grid);
            var outputRow = new GridRowAccessor(output);

            for (var y = 0; y < height; y++)
            {
                inputRow.RowIndex = y;
                outputRow.RowIndex = y;

                for (var x = 1; x < this.grid.Width; x += 3)
                {
                    var rule = this.automaton.FindMatchingRule(inputRow, x);

                    var outputX = x / 3;
                    rule.Apply(outputX, outputRow);
                }
            }

            return output;
        }
    }
}