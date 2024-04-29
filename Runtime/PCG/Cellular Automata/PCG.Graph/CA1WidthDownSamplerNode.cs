using System;
using System.Collections.Generic;
using Chinchillada.CellularAutomata;
using Chinchillada.Grid;
using GraphProcessor;
using UnityEngine;

namespace Chinchillada.PCGraphs.Grid
{
    [Serializable, NodeMenuItem("Ints/Grids/CA1D Width Down Sampler")]
    public class CA1WidthDownSamplerNode : IntGridModifierNode
    {
        [SerializeField, Input] private CellularAutomata1D<int> automaton;


        protected override IEnumerable<Grid2D<int>> Modify(Grid2D<int> grid)
        {
            var width = grid.Width / 3;
            var height = grid.Height;

            var output = new Grid2D<int>(width, height);

            var inputRow = new GridRowAccessor(grid);
            var outputRow = new GridRowAccessor(output);

            for (var y = 0; y < height; y++)
            {
                inputRow.RowIndex = y;
                outputRow.RowIndex = y;

                for (var x = 1; x < grid.Width; x += 3)
                {
                    var rule = this.automaton.FindMatchingRule(inputRow, x);

                    var outputX = x / 3;
                    rule.Apply(outputX, outputRow);
                }
            }

            yield return output;
        }
    }
}