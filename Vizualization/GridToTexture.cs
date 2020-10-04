using Chinchillada.Colorscheme;
using UnityEngine;

namespace Chinchillada.Grid.Visualization
{
    public static class GridToTexture
    {
        public static Texture2D ToTexture(this Grid2D grid, IColorScheme colorScheme, FilterMode filterMode = FilterMode.Point)
        {
            var texture = new Texture2D(grid.Width, grid.Height)
            {
                filterMode = filterMode
            };

            for (var x = 0; x < grid.Width; x++)
            for (var y = 0; y < grid.Height; y++)
            {
                var item = grid[x, y];
                var color = colorScheme[item];

                texture.SetPixel(x, y, color);
            }

            texture.Apply();
            return texture;
        }
    }
}