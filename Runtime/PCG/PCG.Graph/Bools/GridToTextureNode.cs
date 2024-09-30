namespace Chinchillada.Grid.PCGraphs
{
    using System;
    using System.Collections.Generic;
    using Chinchillada.PCGraphs.Noise;
    using GraphProcessor;
    using UnityEngine;

    [Serializable, NodeMenuItem("Grid/Int/To Texture")]
    public class GridToTextureNode : TextureGeneratorNode
    {
        [Input] public Grid2D<bool> grid;

        [Input] public int pixelsPerCell = 10;

        [Input, ShowAsDrawer] public Color trueColor = Color.black;
        [Input, ShowAsDrawer] public Color falseColor = Color.white;
        
        protected override IEnumerable<Texture2D> GenerateTextureAsync()
        {
            yield return this.grid.ToTexture(this.PickColor, this.pixelsPerCell);
        }

        private Color PickColor(bool value) => value ? this.trueColor : this.falseColor;
    }
}