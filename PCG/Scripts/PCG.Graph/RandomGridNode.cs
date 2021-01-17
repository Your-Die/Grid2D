using Chinchillada.Distributions;
using Chinchillada.Generation.Grid;
using Chinchillada.Grid;
using UnityEngine;

namespace Generators.Grid
{
    public class RandomGridNode : GridGeneratorNode
    {
        [SerializeField, Input] private int width = 10;
        [SerializeField, Input] private int height = 10;

        [SerializeField] private IDistribution<int> fillDistribution;

        protected override Grid2D GenerateGrid()
        {
            return RandomGridGenerator.GenerateGrid(this.width, this.height, this.fillDistribution, this.Random);
        }

        protected override void UpdateInputs()
        {
            this.width = this.GetInputValue(nameof(this.width), this.width);
            this.height = this.GetInputValue(nameof(this.height), this.height);
        }
    }
}