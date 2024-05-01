using Chinchillada.Grid;

namespace Chinchillada.PCG.Grid
{
    public interface ICellularRule<T>
    {
        T Apply(int x, int y, Grid2D<T> grid);
    }
}