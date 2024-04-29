using Chinchillada.Grid;

namespace Chinchillada.PCG.Grid
{
    public class RandomGridGenerator : GeneratorBase<Grid2D<int>>
    {
        private readonly int width;
        private readonly int height;

        private readonly IDistribution<int> valueDistribution;

        public RandomGridGenerator(int width, int height, IDistribution<int> valueDistribution)
        {
            this.width = width;
            this.height = height;
            this.valueDistribution = valueDistribution;
        }

        protected override Grid2D<int> GenerateInternal(IRNG random)
        {
            var items = new int[this.width, this.height];

            for (var x = 0; x < this.width; x++)
            for (var y = 0; y < this.height; y++)
                items[x, y] = this.valueDistribution.Sample(random);

            return new Grid2D<int>(items);
        }
    }
}