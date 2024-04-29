using System;
using System.Collections.Generic;
using Chinchillada.CellularAutomata;
using GraphProcessor;
using UnityEngine;

namespace Chinchillada.Grid.PCGraphs
{
    [Serializable, NodeMenuItem("Ints/Grids/CA1 Height Down Sampler")] 
    public class CA1DHeightDownSamplernode : IntGridModifierNode
    {
        [SerializeField, Input] private CellularAutomata1D<int> automaton;

        protected override bool CreateWorkingCopy => false;

        protected override IEnumerable<Grid2D<int>> Modify(Grid2D<int> grid)
        {
            var height = grid.Height / 3;
            var width = grid.Width;
            
            var result = new Grid2D<int>(width, height);

            var inputColumn = new GridColumnAccessor(grid);
            var outputColumn = new GridColumnAccessor(result);

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

            yield return result;
        }
    }
}