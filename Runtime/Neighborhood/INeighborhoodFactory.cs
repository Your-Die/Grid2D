using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chinchillada.Grid
{
    public interface INeighborhoodFactory
    {
        public GridNeighborhood Get(IGrid2D grid, int centerX, int centerY);
    }
    
    public static class NeighborhoodFactoryExtensions
    {
        public static GridNeighborhood Get(this INeighborhoodFactory factory, IGrid2D grid, Vector2Int center)
        {
            return factory.Get(grid, center.x, center.y);
        }

        public static IEnumerable<Vector2Int> GetWithValue<T>(this INeighborhoodFactory factory,
                                                              IGrid2D<T> grid,
                                                              Vector2Int center,
                                                              T value)
        {
            return factory.Get(grid, center).Where(coordinate => value.Equals(grid[coordinate]));
        }
    }
}