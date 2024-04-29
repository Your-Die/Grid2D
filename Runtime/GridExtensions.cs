using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chinchillada.Grid
{
    public static class GridExtensions
    {
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
        
        public static void DrawHorizontal<T>(this Grid2D<T> grid, T value, int y, int xStart, int xEnd)
        {
            for (var x = xStart; x < xEnd; x++)
            {
                grid[x, y] = value;
            }
        }

        public static void DrawVertical<T>(this Grid2D<T> grid, T value, int x, int yStart, int yEnd)
        {
            for (var y = yStart; y < yEnd; y++)
            {
                grid[x, y] = value;
            }
        }
        
        public static Grid2D<TOut> Select<TIn, TOut>(this Grid2D<TIn> grid, Func<TIn, TOut> selector)
        {
            var output = grid.CopyShape<TOut>();

            foreach (var coordinate in grid.Coordinates)
            {
                var value = grid[coordinate];
                output[coordinate] = selector.Invoke(value);
            }

            return output;
        }

        public static Grid2D<T> MirrorX<T>(this Grid2D<T> oldGrid)
        {
            var newGrid = oldGrid.CopyShape();

            for (int oldX = 0; oldX < oldGrid.Width; oldX++)
            {
                int newX = oldGrid.Width - (1 + oldX);

                for (int y = 0; y < oldGrid.Height; y++)
                    newGrid[newX, y] = oldGrid[oldX, y];
            }

            return newGrid;
        }

        public static Grid2D<T> MirrorY<T>(this Grid2D<T> oldGrid)
        {
            var newGrid = oldGrid.CopyShape();

            for (int oldY = 0; oldY < oldGrid.Height; oldY++)
            {
                int newY = oldGrid.Height - (1 + oldY);

                for (int x = 0; x < oldGrid.Width; x++)
                    newGrid[x, newY] = oldGrid[x, oldY];
            }

            return newGrid;
        }

        public static Grid2D<T> RotateLeft<T>(this Grid2D<T> grid)
        {
            int oldHeight = grid.Height;
            int oldWidth = grid.Width;

            var newGrid = new Grid2D<T>(oldHeight, oldWidth);

            int newColumn, newRow = 0;
            for (int oldColumn = oldHeight - 1; oldColumn >= 0; oldColumn--)
            {
                newColumn = 0;
                for (int oldRow = 0; oldRow < oldWidth; oldRow++)
                {
                    newGrid[newRow, newColumn] = grid[oldRow, oldColumn];
                    newColumn++;
                }

                newRow++;
            }

            return newGrid;
        }

        public static Grid2D<T> RotateRight<T>(this Grid2D<T> grid)
        {
            var transposed = grid.Transpose();
            return transposed.MirrorY();
        }

        public static Grid2D<T> Rotate180<T>(this Grid2D<T> grid) => grid.RotateLeft().RotateLeft();

        public static Grid2D<T> Transpose<T>(this Grid2D<T> grid)
        {
            var newGrid = new Grid2D<T>(grid.Height, grid.Width);

            for (int x = 0; x < grid.Width; x++)
            for (int y = 0; y < grid.Height; y++)
            {
                newGrid[x, y] = grid[y, x];
            }

            return newGrid;
        }

        public static Vector2Int RandomCoordinate<T>(this Grid2D<T> grid, IRNG random)
        {
            int x = random.Range(0, grid.Width);
            int y = random.Range(0, grid.Height);

            return new Vector2Int(x, y);
        }

        public static List<Vector2Int> FloodFill(this Grid2D<int> grid,
                                                        Vector2Int start,
                                                        int empty,
                                                        int fill,
                                                        INeighborhoodFactory neighborhoodFactory)
        {
            var frontier = new Stack<Vector2Int>();
            var visited = new HashSet<Vector2Int>();

            frontier.Push(start);
            visited.Add(start);

            var output = new List<Vector2Int>();
            while (frontier.Any())
            {
                Vector2Int coordinate = frontier.Pop();

                GridNeighborhood neighborhood = neighborhoodFactory.Get(grid, coordinate);
                foreach (Vector2Int neighbor in neighborhood)
                {
                    int value = grid[neighbor];

                    if (value == empty && !visited.Contains(neighbor))
                    {
                        frontier.Push(neighbor);
                        visited.Add(neighbor);
                    }
                }

                grid[coordinate] = fill;
                output.Add(coordinate);
            }

            return output;
        }
    }
}