using System;
using System.Collections.Generic;
using GraphProcessor;

namespace Chinchillada.Grid.PCGraphs
{
    [Serializable, NodeMenuItem("Grid/Bool/To Int")]
    public class BoolToIntNode : IntGridGeneratorNode
    {
        [Input] public Grid2D<bool> inputGrid;

        [Input, ShowAsDrawer] public int trueValue = 1;
        [Input, ShowAsDrawer] public int falseValue = 0;
        
        protected override IEnumerable<Grid2D<int>> GenerateAsync()
        {
            yield return this.inputGrid.Select(boolValue => boolValue ? this.trueValue : this.falseValue);
        }
    }
}