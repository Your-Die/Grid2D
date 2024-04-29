using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chinchillada.Grid
{
    public static class GridExtensions
    {
        public static IEnumerable<(Vector2Int, Direction)> GetNeighbors<T>(this IGrid2D<T> grid, Vector2Int node)
        {
            if (node.x > 0) yield return (new Vector2Int(node.x                       - 1, node.y), Direction.West);
            if (node.y > 0) yield return (new Vector2Int(node.x, node.y               - 1), Direction.South);
            if (node.x < grid.Width  - 1) yield return (new Vector2Int(node.x         + 1, node.y), Direction.East);
            if (node.y < grid.Height - 1) yield return (new Vector2Int(node.x, node.y + 1), Direction.South);
        }

        public static IEnumerable<T> GetWindow<T>(this IGrid2D<T> grid, int topLeftX, int topLeftY, Vector2Int window)
        {
            for (var x = 0; x < window.x; x++)
            for (var y = 0; y < window.y; y++)
            {
                var windowX = topLeftX + x;
                var windowY = topLeftY + y;

                yield return grid[windowX, windowY];
            }
        }

        public static IEnumerable<Vector2Int> GetCoordinates<T>(this IGrid2D<T> grid)
        {
            for (var x = 0; x < grid.Width; x++)
            for (var y = 0; y < grid.Height; y++)
                yield return new Vector2Int(x, y);
        }

        public static Vector2Int ClampToBounds(this IGrid2D<int> grid, Vector2Int coordinate)
        {
            coordinate.x = Mathf.Clamp(coordinate.x, 0, grid.Width  - 1);
            coordinate.y = Mathf.Clamp(coordinate.y, 0, grid.Height - 1);

            return coordinate;
        }

        public static Vector2Int ChooseRandomCoordinate(this IGrid2D<int> grid, IRNG numberGenerator = null)
        {
            numberGenerator ??= new UnityRandom();

            return new Vector2Int
            {
                x = numberGenerator.Range(grid.Width),
                y = numberGenerator.Range(grid.Height)
            };
        }

        public static ReadOnlyGrid<T> AReadOnly<T>(this IGrid2D<T> grid) => new(grid);
        
        public static Texture2D ToTexture<T>(this Grid2D<T> grid, Func<T, Color> colorPicker, int pixelsPerCell)
        {
            var width = grid.Width * pixelsPerCell;
            var height = grid.Height * pixelsPerCell;

            var texture = new Texture2D(width, height);

            for (var x = 0; x < grid.Width; x++)
            for (var y = 0; y < grid.Height; y++)
            {
                var value = grid[x, y];
                var color = colorPicker.Invoke(value);

                for (var xOffset = 0; xOffset < pixelsPerCell; xOffset++)
                for (var yOffset = 0; yOffset < pixelsPerCell; yOffset++)
                {
                    var pixelX = x * pixelsPerCell + xOffset;
                    var pixelY = y * pixelsPerCell + yOffset;

                    texture.SetPixel(pixelX, pixelY, color);
                }
            }

            texture.Apply();
            return texture;
        }
    }
}