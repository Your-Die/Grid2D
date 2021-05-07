using System.Linq;
using Chinchillada.Colorscheme;
using Chinchillada;
using Chinchillada.Grid;
using UnityEngine;

namespace Generators.Grid
{
    public class TextureToGrid : GridGeneratorNode
    {
        [SerializeField, Input] private Texture2D texture;

        [SerializeField, Output] private ColorScheme colorScheme;
        
        protected override void UpdateInputs()
        {
            this.texture = this.GetInputValue(nameof(this.texture), this.texture);
        }

        protected override Grid2D GenerateGrid()
        {
            this.BuildColorScheme();
            var colorDictionary = this.colorScheme.IndexDictionary();

            var grid = new Grid2D(this.texture.width, this.texture.height);
            
            for (var x = 0; x < this.texture.width; x++)
            for (var y = 0; y < this.texture.height; y++)
            {
                var pixel = this.texture.GetPixel(x, y);
                var colorIndex = colorDictionary[pixel];

                grid[x, y] = colorIndex;
            }

            return grid;
        }

        private void BuildColorScheme()
        {
            var pixels = this.texture.GetPixels();

            // Sort color by frequency.
            var colors = pixels.ToLookup(pixel => pixel)        // Group by color
                               .OrderBy(group => @group.Count()) // Sort on frequency
                               .Select(group => @group.Key)      // Select the colors.
                               .ToArray();                      // Output to array. 

            this.colorScheme = new ColorScheme(colors);
        }
    }
}