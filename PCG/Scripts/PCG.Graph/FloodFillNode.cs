using System;
using System.Collections.Generic;
using System.Linq;
using Chinchillada.Distributions;
using Chinchillada.Generation.Grid;
using Chinchillada.Grid;
using UnityEngine;
using xNode;

namespace Generators.Grid
{
    using Chinchillada;

    public class FloodFillNode : GridGeneratorNode
    {
        [SerializeField, Input] private Grid2D grid;

        [SerializeField, Output] private int regionCount;

        [SerializeField] private IValueSelector valueSelector = new IncrementalValues();

        protected override void UpdateInputs()
        {
            this.grid = this.GetInputValue(nameof(this.grid), this.grid);
        }

        public override object GetValue(NodePort port)
        {
            return port.fieldName == nameof(this.regionCount) 
                ? this.regionCount 
                : base.GetValue(port);
        }

        protected override Grid2D GenerateGrid()
        {
            var output = this.InitializeGrid();
            this.regionCount = 0;

            for (var x = 0; x < this.grid.Width; x++)
            for (var y = 0; y < this.grid.Height; y++)
            {
                if (output[x, y] >= 0)
                    continue;

                var value = this.valueSelector.SelectValue(this.regionCount++, this.Random);
                FloodFill(output, x, y, value);
            }

            return output;
        }

        private static void FloodFill(Grid2D grid, int x, int y, int value)
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

        private Grid2D InitializeGrid()
        {
            var output = this.grid.CopyShape();

            for (var x = 0; x < this.grid.Width; x++)
            for (var y = 0; y < this.grid.Height; y++)
                output[x, y] = -this.grid[x, y] - 1;

            return output;
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