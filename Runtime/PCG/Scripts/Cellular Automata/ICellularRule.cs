using Chinchillada.Grid;

namespace Chinchillada.PCG.Grid
{
    public interface ICellularRule
    {
        int Apply(int x, int y, Grid2D<int> grid);
    }
}