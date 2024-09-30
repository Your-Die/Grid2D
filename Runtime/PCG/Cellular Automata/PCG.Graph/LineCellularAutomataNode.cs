using System;
using System.Collections.Generic;
using System.Linq;
using Chinchillada.Grid.PCGraphs;
using GraphProcessor;

namespace Chinchillada.Grid
{
    [NodeMenuItem("Grid/Line Cellular Automata"), Serializable]
    public class LineCellularAutomataNode : IntGridGeneratorNode
    {
        [Input, ShowAsDrawer] public int height;

        [Input, ShowAsDrawer] public List<int> firstRow;

        [Input] public List<LineCellularRule<int>> rules;

        public override int ExpectedIterations => this.height;

        protected override IEnumerable<Grid2D<int>> GenerateAsync()
        {
            var ruleSet = new LineCellularRuleSet<int>(this.rules);
            
            var grid = CreateGridFrom(this.firstRow, this.height);
            yield return grid;

            for (var y = 1; y < this.height; y++)
            {
                ruleSet.GenerateLineAtHeight(grid, y);
                yield return grid;
            }
        }

        private static Grid2D<T> CreateGridFrom<T>(IReadOnlyList<T> row, int height)
        {
            var width = row.Count;
            var grid = new Grid2D<T>(width, height);

            for (var x = 0; x < width; x++) 
                grid[x, 0] = row[x];

            return grid;
        }
        
        
        [CustomPortBehavior(nameof(rules))]
        IEnumerable<PortData> GetPortsForInputs(List<SerializableEdge> edges)
        {
            yield return new PortData
            {
                displayName = nameof(this.rules),
                displayType = typeof(LineCellularRule<int>),
                acceptMultipleEdges = true
            };
        }

        [CustomPortInput(nameof(rules), typeof(LineCellularRule<int>), allowCast = true)]
        public void GetInputs(List<SerializableEdge> edges)
        {
            rules = edges.Select(e => (LineCellularRule<int>)e.passThroughBuffer).ToList();
        }
    }
}