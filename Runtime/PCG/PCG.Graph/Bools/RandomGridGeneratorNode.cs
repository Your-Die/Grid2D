namespace Chinchillada.Grid.PCGraphs
{
    using System;
    using System.Collections.Generic;
    using Chinchillada;
    using GraphProcessor;

    [Serializable, NodeMenuItem("Grid/Bool/Random")]
    public class RandomGridGeneratorNode : BoolGridGeneratorNode, IUsesRNG
    {
        [Input, ShowAsDrawer] public IDistribution<bool> valueDistribution;

        [Input, ShowAsDrawer] public int width  = 10;
        [Input, ShowAsDrawer] public int height = 10;

        public override int ExpectedIterations => this.width * this.height;

        public IRNG RNG { get; set; }

        public override Grid2D<bool> Generate()
        {
            var grid = new Grid2D<bool>(this.width, this.height);

            for (var x = 0; x < this.width; x++)
            for (var y = 0; y < this.height; y++)
                grid[x, y] = this.valueDistribution.Sample(this.RNG);

            return grid;
        }

        protected override IEnumerable<Grid2D<bool>> GenerateAsync()
        {
            yield return this.Generate();
        }
    }
}