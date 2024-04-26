using System;
using System.Collections.Generic;
using Chinchillada.Grid;
using GraphProcessor;
using UnityEngine;

namespace Chinchillada.PCGraphs.Grid
{
    [Serializable, NodeMenuItem("Ints/Grids/Uniform")]
    public class UniformGridNode : IntGridGeneratorNode, IUsesRNG
    {
        [SerializeField, Input, ShowAsDrawer] private int width;
        [SerializeField, Input, ShowAsDrawer] private int height;

        [SerializeField, Input, ShowAsDrawer] private int value;
        public IRNG RNG { get; set; }

        protected override IEnumerable<Grid2D<int>> GenerateAsync()
        {
            yield return new Grid2D<int>(this.width, this.height, this.value);
        }
    }
}