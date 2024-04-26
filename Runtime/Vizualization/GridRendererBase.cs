using System;
using Chinchillada;

namespace Chinchillada.Grid.Visualization
{
    
    public abstract class GridRendererBase<T> : AutoRefBehaviour, IGridRenderer<T> 
    {
        public Grid2D<T> Grid { get; private set; }
        
        public event Action<Grid2D<T>> NewGridRegistered;

        public void Render(Grid2D<T> grid)
        {
            this.Grid = grid;
            this.NewGridRegistered?.Invoke(grid);
            
            this.RenderGrid(grid);
        }

        protected abstract void RenderGrid(Grid2D<T> newGrid);
    }
}