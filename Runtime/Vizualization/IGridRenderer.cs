using System;

namespace Chinchillada.Grid.Visualization
{
    public interface IGridRenderer<T>
    {
        Grid2D<T> Grid { get; }

        event Action<Grid2D<T>> NewGridRegistered;


        /// <summary>
        /// Render the <paramref name="grid"/>.
        /// </summary>
        void Render(Grid2D<T> grid);
    }
}

