using Chinchillada.Grid;

namespace Chinchillada.PCG.Grid
{
    public interface ICellularRule<T>
    {
        bool Match(int x, int y, Grid2D<T> grid, out T result);
    }
}