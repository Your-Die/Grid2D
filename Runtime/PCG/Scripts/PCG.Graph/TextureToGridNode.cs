using System;
using System.Collections.Generic;
using System.Linq;
using Chinchillada.Grid;
using GraphProcessor;
using UnityEngine;

namespace Chinchillada.PCGraphs.Grid
{
    [Serializable, NodeMenuItem("Ints/Grids/From Texture")]
    public class TextureToGridNode : IntGridGeneratorNode
    {
        [SerializeField, Input] private Texture2D texture;


        protected override IEnumerable<Grid2D<int>> GenerateAsync()
        {
            var colorDictionary = this.BuildColorScheme();

            var grid = new Grid2D<int>(this.texture.width, this.texture.height);

            for (var x = 0; x < this.texture.width; x++)
            for (var y = 0; y < this.texture.height; y++)
            {
                var pixel = this.texture.GetPixel(x, y);
                var colorIndex = colorDictionary[pixel];

                grid[x, y] = colorIndex;
            }

            yield return grid;
        }

        private IDictionary<Color, int> BuildColorScheme()
        {
            var pixels = this.texture.GetPixels();

            // Sort color by frequency.
            return pixels.ToLookup(pixel => pixel) // Group by color
                .OrderBy(group => @group.Count()) // Sort on frequency
                .Select(group => @group.Key) // Select the colors.
                .ToList()
                .IndexDictionary(); // Output to array. 
        }
    }
}