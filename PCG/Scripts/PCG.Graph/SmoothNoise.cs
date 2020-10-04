using Chinchillada.Generation.Grid;
using Chinchillada.Grid;
using UnityEngine;

namespace Chinchillada.GeneratorGraph.Grid
{
    public class SmoothNoise : GridGeneratorNode
    {
        [SerializeField, Input] private Grid2D grid;

        [SerializeField, Input] private int samplePeriod;
        
        protected override void UpdateInputs()
        {
            this.grid = this.GetInputValue(nameof(this.grid), this.grid);
            this.samplePeriod = this.GetInputValue(nameof(this.samplePeriod), this.samplePeriod);
        }

        protected override Grid2D GenerateGrid() => SmoothNoiseGenerator.SmoothNoise(this.grid, this.samplePeriod);
    }
}