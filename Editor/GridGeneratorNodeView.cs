using Chinchillada.Grid;
using JetBrains.Annotations;

namespace Chinchillada.PCGraphs.Editor
{
    using UnityEngine;

    public abstract class GridGeneratorNodeView<TNode, TValue> : GeneratorNodeViewWithTexture<TNode, Grid2D<TValue>>
        where TNode : GeneratorNode<Grid2D<TValue>>
    {
        private const int PixelsPerCell = 10;

        protected override Texture2D GenerateTexture(Grid2D<TValue> grid)
        {
            return grid.ToTexture(this.PickColor, PixelsPerCell);
        }

        protected abstract Color PickColor(TValue value);
    }
}