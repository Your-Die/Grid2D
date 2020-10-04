using Chinchillada.Colorscheme;
using Chinchillada.Foundation;
using Chinchillada.Grid;
using Chinchillada.Grid.Visualization;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Chinchillada.GeneratorGraph.Grid
{
    public abstract class GridGeneratorNode : GeneratorNode<Grid2D>
    {
        [SerializeField, ReadOnly, UsedImplicitly, HideLabel, PropertyOrder(int.MaxValue), PreviewField(100, ObjectFieldAlignment.Center)]
        private Texture2D previewTexture;

        [SerializeField, DefaultAsset("ColorScheme"), FoldoutGroup("Preview Settings")]
        private IColorScheme previewColorScheme = new ColorScheme(Color.black, Color.white);

        [SerializeField, FoldoutGroup("Preview Settings")]
        private FilterMode previewFilterMode = FilterMode.Point;

        protected override Grid2D GenerateInternal() => this.GenerateGrid();

        protected abstract Grid2D GenerateGrid();

        protected override void RenderPreview(Grid2D result)
        {
            this.previewTexture = result.ToTexture(this.previewColorScheme, this.previewFilterMode);
        }
    }
}