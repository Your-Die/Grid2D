using System;
using System.Collections.Generic;
using Chinchillada.Grid;
using GraphProcessor;
using UnityEngine;

namespace Chinchillada.Grid.PCGraphs
{
    [Serializable, NodeMenuItem("Ints/Grids/Random")]
    public class RandomGridNode : IntGridGeneratorNode, IUsesRNG
    {
        [SerializeField, Input] private int width = 10;
        [SerializeField, Input] private int height = 10;

        [SerializeField, Input] private IDistribution<int> fillDistribution;

        public IRNG RNG { get; set; }

        protected override IEnumerable<Grid2D<int>> GenerateAsync()
        {
            yield return new Grid2D<int>(this.width, this.height, () => this.fillDistribution.Sample(this.RNG));
        }
    }
}