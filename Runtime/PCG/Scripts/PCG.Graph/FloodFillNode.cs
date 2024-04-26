using System;
using System.Collections.Generic;
using System.Linq;
using Chinchillada.Grid;
using GraphProcessor;
using UnityEngine;

namespace Chinchillada.PCGraphs.Grid
{
    using Chinchillada;

    [Serializable, NodeMenuItem("Ints/Grids/Flood Fill")]
    public class FloodFillNode : IntGridModifierNode, IUsesRNG
    {
        [Input] public IValueSelector valueSelector = new IncrementalValues();

        [SerializeField, Output] private int regionCount;

        public IRNG RNG { get; set; }

        public override int ExpectedIterations => this.inputGrid.Width * this.inputGrid.Height;
        protected override bool CreateWorkingCopy => false;

        protected override IEnumerable<Grid2D<int>> Modify(Grid2D<int> grid)
        {
            var outputGrid = InitializeGrid(grid);
            this.regionCount = 0;

            for (var x = 0; x < grid.Width; x++)
            for (var y = 0; y < grid.Height; y++)
            {
                if (outputGrid[x, y] >= 0)
                    continue;

                var value = this.valueSelector.SelectValue(this.regionCount++, this.RNG);
                FloodFill(outputGrid, x, y, value);
                yield return outputGrid;
            }

            yield return outputGrid;
        }

        private static void FloodFill(Grid2D<int> grid, int x, int y, int value)
        {
            var queue = new Queue<Vector2Int>();

            var startValue = grid[x, y];
            var startNode = new Vector2Int(x, y);
            queue.Enqueue(startNode);

            while (queue.Any())
            {
                var node = queue.Dequeue();
                grid[node.x, node.y] = value;

                foreach (var (neighbor, _) in grid.GetNeighbors(node))
                {
                    var neighborValue = grid[neighbor.x, neighbor.y];

                    if (neighborValue.Equals(startValue))
                        queue.Enqueue(neighbor);
                }
            }
        }

        private static Grid2D<int> InitializeGrid(Grid2D<int> grid)
        {
            var outputGrid = grid.CopyShape();

            for (var x = 0; x < grid.Width; x++)
            for (var y = 0; y < grid.Height; y++)
                outputGrid[x, y] = -grid[x, y] - 1;

            return outputGrid;
        }
    }

    public interface IValueSelector
    {
        int SelectValue(int regionIndex, IRNG random);
    }

    [Serializable]
    public class IncrementalValues : IValueSelector
    {
        public int SelectValue(int regionIndex, IRNG random) => regionIndex;
    }

    [Serializable]
    public class DistributionValues : IValueSelector
    {
        [SerializeField] private IDistribution<int> values;

        public int SelectValue(int regionIndex, IRNG random) => this.values.Sample(random);
    }

    [Serializable]
    public class RingBuffer : IValueSelector
    {
        [SerializeField] private IList<int> values;

        public int SelectValue(int regionIndex, IRNG random)
        {
            var valueIndex = regionIndex % this.values.Count;
            return this.values[valueIndex];
        }
    }
}