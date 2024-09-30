using System.Collections.Generic;
using GraphProcessor;

namespace Chinchillada.Grid.PCGraphs
{
    public abstract class IntGridModifierNode : IntGridGeneratorNode
    {
        [Input] public Grid2D<int> inputGrid;

        /// <summary>
        /// Whether we should create a copy of the input grid to work on.
        /// Can be overwritten to false if we're doing readonly-operations on the input grid.
        /// </summary>
        protected virtual bool CreateWorkingCopy => true;

        protected override IEnumerable<Grid2D<int>> GenerateAsync()
        {
            var grid = this.CreateWorkingCopy ? this.inputGrid.Copy() : this.inputGrid;
            return this.Modify(grid);
        }

        protected abstract IEnumerable<Grid2D<int>> Modify(Grid2D<int> grid);
    }
}