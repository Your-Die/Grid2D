using System;
using System.Collections.Generic;
using Chinchillada.PCGraphs;
using GraphProcessor;
using UnityEngine;

namespace Chinchillada.Grid.PCGraphs
{
    [Serializable, NodeMenuItem("Grid/Int/Down Sampler")]
    public class DownSamplerNode : IntGridGeneratorNode
    {
        [SerializeField, Input] private Grid2D<int> grid;

        [SerializeField, Input, ShowAsDrawer] private Vector2Int windowShape;

        [Input] public IWindowSampler<int> windowSampler;

        protected override IEnumerable<Grid2D<int>> GenerateAsync()
        {
            var shape = this.grid.Shape.DivideElementWise(this.windowShape);
            var outputGrid = new Grid2D<int>(shape);

            var windowX = this.windowShape.x;
            var windowY = this.windowShape.y;

            var maxX = this.grid.Width - windowX;
            var maxY = this.grid.Height - windowY;

            for (var x = 0; x < maxX; x += windowX)
            for (var y = 0; y < maxY; y += windowY)
            {
                var outputX = x / windowX;
                var outputY = y / windowY;

                outputGrid[outputX, outputY] = this.windowSampler.Sample(this.grid, x, y, this.windowShape);
            }

            yield return outputGrid;
        }
    }

    [Serializable, NodeMenuItem("Grid/Int/Window Sampler")]
    public class IntWindowSamplerNode : GeneratorNode<IWindowSampler<int>>
    {
        [ShowAsDrawer, Input] public Sampler samplerType;

        public enum Sampler
        {
            RoundedMean,
            Mode,
            Random
        }


        public override IWindowSampler<int> Generate()
        {
            return this.samplerType switch
            {
                Sampler.RoundedMean => new RoundedMeanSampler(),
                Sampler.Mode        => new ModeSampler<int>(),
                Sampler.Random      => new RandomSampler<int>(),

                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}