namespace Chinchillada.Generation.Grid
{
    using System;
    using Chinchillada.Grid;
    using UnityEngine;

    [Serializable]
    public class EmptyGridGenerator : GeneratorBase<Grid2D>
    {
        [SerializeField] private int width;

        [SerializeField] private int height;
        
        protected override Grid2D GenerateInternal() => new Grid2D(this.width, this.height);
    }
}