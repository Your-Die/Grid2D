using Chinchillada.Generation.Grid;
using Chinchillada.Grid;
using UnityEngine;

namespace Generators.Grid
{
    public class EmptyGridNode : GridGeneratorNode
    {
        [SerializeField, Input] private int width;
        [SerializeField, Input] private int height;

        protected override Grid2D GenerateGrid()
        {
            return new Grid2D(this.width, this.height);
        }

        protected override void UpdateInputs()
        {
            this.width = this.GetInputValue(nameof(this.width), this.width);
            this.height = this.GetInputValue(nameof(this.height), this.height);
        }
    }
}