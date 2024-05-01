using System;
using System.Collections.Generic;
using Chinchillada.PCG.Grid;
using Chinchillada.Grid;
using GraphProcessor;
using UnityEngine;

namespace Chinchillada.Grid.PCGraphs
{
    [Serializable, NodeMenuItem("Grid/Int/Randomized Zoom")]
    public class RandomizedZoomNode : IntGridModifierNode, IUsesRNG
    {
        [SerializeField, Input, ShowAsDrawer] private int iterations = 1;

        
        protected override bool CreateWorkingCopy => false;
        public IRNG RNG { get; set; }

        protected override IEnumerable<Grid2D<int>> Modify(Grid2D<int> grid)
        {
            for (int i = 0; i < this.iterations; i++)
            {
                grid = RandomizedGridZoom.RandomizedZoom(grid, this.RNG);
                yield return grid;
            }
        }

    }
}