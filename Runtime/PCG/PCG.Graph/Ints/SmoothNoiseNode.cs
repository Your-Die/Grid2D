using System;
using System.Collections.Generic;
using Chinchillada.PCG.Grid;
using GraphProcessor;
using UnityEngine;

namespace Chinchillada.Grid.PCGraphs
{
    [Serializable, NodeMenuItem("Grid/Int/Smooth Noise")]
    public class SmoothNoiseNode : IntGridModifierNode
    {
        [SerializeField, Input] private int samplePeriod;
        
        protected override IEnumerable<Grid2D<int>> Modify(Grid2D<int> grid)
        {
            yield return SmoothNoise.Generate(grid, this.samplePeriod);
        }
    }
}