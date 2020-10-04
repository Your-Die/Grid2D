using System;
using System.Collections.Generic;
using System.Linq;
using Chinchillada.Distributions;
using Chinchillada.Foundation;
using Chinchillada.Grid;
using Foundation.Algorithms;
using UnityEngine;

namespace Chinchillada.Generation.WaveformCollapse
{
    public class SimpleTiledModel
    {
        private readonly IDictionary<int, WaveRule> rules;

        public SimpleTiledModel(Grid2D input)
        {
            this.rules = BuildRules(input);
        }

        public Grid2D Generate(Vector2Int shape)
        {
            var grid = new Grid2D(shape);

            var (uncollapsed, cellLookup) = BuildCells();

            var wave = new Stack<WaveCell>();

            while (uncollapsed.Any())
            {
                var frontierCell = uncollapsed.First();
                wave.Push(frontierCell);

                while (wave.Any())
                {
                    var cell = wave.Pop();
                    Collapse(cell);
                }
            }

            return grid;

            void Collapse(WaveCell cell)
            {
                uncollapsed.Remove(cell);
                var state = cell.Collapse(this.rules);
                grid[cell.X, cell.Y] = state;

                foreach (var (neighbor, direction) in grid.GetNeighbors(cell.Coordinate))
                {
                    var neighborCell = cellLookup[neighbor.x, neighbor.y];
                    var inverseDirection = direction.Inverse();

                    foreach (var possibleState in neighborCell.PossibleStates.ToArray())
                    {
                        var rule = this.rules[possibleState];
                        
                        if (!rule.ValidateNeighbor(inverseDirection, state))
                            neighborCell.PossibleStates.Remove(possibleState);
                    }

                    neighborCell.UpdateEntropy(this.GetStateWeight);
                }
            }

            (SortedSet<WaveCell>, WaveCell[,]) BuildCells()
            {
                var possibleStates = this.rules.Keys;

                var lookup = new WaveCell[shape.x, shape.y];
                var sortedSet = new SortedSet<WaveCell>(WaveCell.Comparer);

                for (var x = 0; x < shape.x; x++)
                for (var y = 0; y < shape.y; y++)
                {
                    var cell = new WaveCell(x, y, possibleStates);

                    lookup[x, y] = cell;
                    sortedSet.Add(cell);
                }

                return (sortedSet, lookup);
            }
        }
        
        private float GetStateWeight(int state)
        {
            var rule = this.rules[state];
            return rule.ValueOccurrences;
        }
        
        private static IDictionary<int, WaveRule> BuildRules(Grid2D input)
        {
            var rules = new DefaultDictionary<int, WaveRule>(value => new WaveRule(value) {ValueOccurrences = 1});

            for (var x = 0; x < input.Width; x++)
            for (var y = 0; y < input.Height; y++)
            {
                RegisterValue(x, y);

                if (y > 0)
                    RegisterNorth(x, y);
                if (x > 0)
                    RegisterWest(x, y);
            }

            return rules;

            void RegisterValue(int x, int y)
            {
                var value = input[x, y];
                var rule = rules[value];

                rule.ValueOccurrences++;
            }

            void RegisterNorth(int x, int y)
            {
                var value = input[x, y];
                var neighbor = input[x, y - 1];

                var rule = rules[value];
                var neighborRule = rules[neighbor];

                rule.RegisterNeighbor(neighbor, Direction.North);
                neighborRule.RegisterNeighbor(neighbor, Direction.South);
            }

            void RegisterWest(int x, int y)
            {
                var value = input[x, y];
                var neighbor = input[x - 1, y];

                var rule = rules[value];
                var neighborRule = rules[neighbor];

                rule.RegisterNeighbor(neighbor, Direction.West);
                neighborRule.RegisterNeighbor(neighbor, Direction.East);
            }
        }

        private class WaveRule
        {
            private readonly int value;

            public int ValueOccurrences { get; set; }

            private readonly IDictionary<int, int> northNeighbors = new DefaultDictionary<int, int>(0);
            private readonly IDictionary<int, int> eastNeighbors = new DefaultDictionary<int, int>(0);
            private readonly IDictionary<int, int> southNeighbors = new DefaultDictionary<int, int>(0);
            private readonly IDictionary<int, int> westNeighbors = new DefaultDictionary<int, int>(0);

            public WaveRule(int value)
            {
                this.value = value;
            }

            public void RegisterNeighbor(int neighbor, Direction direction)
            {
                var neighborDictionary = this.GetNeighborDictionary(direction);
                neighborDictionary[neighbor]++;
            }
            
            public bool ValidateNeighbor(Direction direction, int state)
            {
                var neighbors = this.GetNeighborDictionary(direction);
                return neighbors.Keys.Contains(state);
            }

            private IDictionary<int, int> GetNeighborDictionary(Direction direction)
            {
                switch (direction)
                {
                    case Direction.North: return this.northNeighbors;
                    case Direction.East:  return this.eastNeighbors;
                    case Direction.South: return this.southNeighbors;
                    case Direction.West:  return this.westNeighbors;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
                }
            }
        }

        private class WaveCell
        {
            public int X { get; }
            public int Y { get; }

            public Vector2Int Coordinate { get; }
            public IList<int> PossibleStates { get; }

            public float CurrentEntropy { get; set; }

            public static readonly IComparer<WaveCell> Comparer = new EntropyComparer();

            public WaveCell(int x, int y, IEnumerable<int> possibleStates)
            {
                this.X = x;
                this.Y = y;

                this.Coordinate = new Vector2Int(x, y);

                this.PossibleStates = possibleStates.ToList();
            }

            public void UpdateEntropy(Func<int, float> weightFunction)
            {
                var weights = this.PossibleStates.Select(weightFunction);
                this.CurrentEntropy = Entropy.Shannon(weights);
            }

            private class EntropyComparer : IComparer<WaveCell>
            {
                public int Compare(WaveCell x, WaveCell y) => x.CurrentEntropy.CompareTo(y.CurrentEntropy);
            }

            public int Collapse(IDictionary<int, WaveRule> rules)
            {
                var distribution = this.PossibleStates.ToWeighted(GetRuleWeight);
                return distribution.Sample();

                int GetRuleWeight(int state) => rules[state].ValueOccurrences;
            }
        }
    }
}