using Chinchillada.Generation.WaveformCollapse;
using Chinchillada.Grid;
using UnityEngine;

namespace Chinchillada.GeneratorGraph.Grid
{
    public class WaveformCollapseNode : GridGeneratorNode
    {
        [SerializeField, Input] private Grid2D grid;

        [SerializeField, Input] private Vector2Int shape;
        
        private SimpleTiledModel model;
        
        protected override void UpdateInputs()
        {
            var newGrid = this.GetInputValue(nameof(this.grid), this.grid);
            
            if (newGrid != this.grid || this.model == null) 
                this.model = new SimpleTiledModel(newGrid);

            this.grid = newGrid;
        }

        protected override Grid2D GenerateGrid() => this.model.Generate(this.shape);
    }
}