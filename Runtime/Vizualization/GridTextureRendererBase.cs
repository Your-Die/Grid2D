using UnityEngine;

namespace Chinchillada.Grid.Visualization
{
    public abstract class GridTextureRendererBase : GridRendererBase<int>, IIntGridRenderer
    {
        [SerializeField] private IColorScheme colorScheme;
        [SerializeField] protected FilterMode filterMode;

        protected override void RenderGrid(Grid2D<int> newGrid)
        {
            var texture = this.ConvertToTexture(newGrid);
            this.SetTexture(texture);
        }

        private Texture2D ConvertToTexture(Grid2D<int> grid) => grid.ToTexture(this.colorScheme, this.filterMode);

        protected abstract void SetTexture(Texture texture);
    }
}