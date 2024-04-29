using Chinchillada;
using UnityEngine;

namespace Chinchillada.Grid
{
    public interface IGrid2D
    {
        int Height { get; }
        int Width { get; }
    }

    public interface IGrid2D<T> : IGrid2D
    {
        T this[int        x, int y] { get; set; }
        T this[Vector2Int position] { get; set; }
    }
}