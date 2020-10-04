using Chinchillada.Grid;

namespace Chinchillada.Generation.Grid
{
    public interface ICellularRule
    {
        int Apply(int x, int y, Grid2D grid);
    }
}