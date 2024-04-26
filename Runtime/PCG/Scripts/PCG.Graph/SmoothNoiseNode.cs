using System;
using System.Collections.Generic;
using Chinchillada.Grid;
using Chinchillada.PCG.Grid;
using GraphProcessor;
using UnityEngine;

namespace Chinchillada.PCGraphs.Grid
{
    [Serializable,NodeMenuItem("Ints/Grids/Smooth Noise")]
    public class SmoothNoiseNode : IntGridModifierNode
    {
        [SerializeField, Input] private int samplePeriod;
        
        protected override IEnumerable<Grid2D<int>> Modify(Grid2D<int> grid)
        {
            yield return SmoothNoise.Generate(grid, this.samplePeriod);
        }
    }
}