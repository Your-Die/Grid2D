using Chinchillada.Grid;

namespace Chinchillada.PCGraphs.Editor
{
    using System.Collections.Generic;
    using System.Linq;
    using GraphProcessor;
    using Grid;
    using JetBrains.Annotations;
    using UnityEngine;

    [UsedImplicitly, NodeCustomEditor(typeof(IntGridGeneratorNode))]
    public class IntGridGeneratorNodeView :GridGeneratorNodeView<IntGridGeneratorNode,int>
    {
        private readonly Dictionary<int, Color> colorsByValue = new();

        protected override Texture2D GenerateTexture(Grid2D<int> grid)
        {
            this.PrepareColors(grid);
            return base.GenerateTexture(grid);
        }

        private void PrepareColors(Grid2D<int> grid)
        {
            this.colorsByValue.Clear();
            
            var distinctValues = grid.Distinct().ToArray();

            for (var index = 0; index < distinctValues.Length; index++)
            {
                var hue = 1f / index;
                var hsv = new HSVColor(hue, 1, 1);

                var value = distinctValues[index];
                this.colorsByValue[value] = hsv.ToRGB();
            }
        }

        protected override Color PickColor(int value) => this.colorsByValue[value];
    }
}