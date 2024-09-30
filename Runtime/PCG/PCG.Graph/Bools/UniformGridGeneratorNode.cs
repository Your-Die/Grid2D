namespace Chinchillada.Grid.PCGraphs
{
    using System;
    using System.Collections.Generic;
    using GraphProcessor;

    [Serializable, NodeMenuItem("Grid/Bool/Uniform")]
    public class UniformGridGeneratorNode : BoolGridGeneratorNode
    {
        [Input, ShowAsDrawer] public int width  = 10;
        [Input, ShowAsDrawer] public int height = 10;

        [Input, ShowAsDrawer] public bool gridValue = true;

        protected override IEnumerable<Grid2D<bool>> GenerateAsync()
        {
            yield return new Grid2D<bool>(this.width, this.height, this.gridValue);
        }
    }
}