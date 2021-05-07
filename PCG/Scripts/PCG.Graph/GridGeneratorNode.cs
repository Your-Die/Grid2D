using Chinchillada.Colorscheme;
using Chinchillada;
using Chinchillada.Grid;
using Chinchillada.Grid.Visualization;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Generators.Grid
{
    public abstract class GridGeneratorNode : GeneratorNodeWithTexture<Grid2D>
    {
        [SerializeField, DefaultAsset("ColorScheme"), FoldoutGroup(PreviewGroup)]
        private IColorScheme previewColorScheme = new ColorScheme(Color.black, Color.white);
        
        protected override Grid2D GenerateInternal() => this.GenerateGrid();

        protected abstract Grid2D GenerateGrid();

        protected override Texture2D RenderPreviewTexture(Grid2D result) => result?.ToTexture(this.previewColorScheme);
    }
    
    
}