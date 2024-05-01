using System;
using System.Collections.Generic;
using System.Linq;
using GraphProcessor;
using Chinchillada.PCG.Grid;
using UnityEngine.Assertions;

namespace Chinchillada.Grid.PCGraphs
{
    [Serializable, NodeMenuItem("Grid/Int/Cellular Automata/Automata")]
    public class CellularAutomataNode : IntGridModifierNode
    {
        [Input] public List<ICellularRule<int>> rules;

        [Input, ShowAsDrawer] public int iterations = 4;

        public override int ExpectedIterations => this.iterations;

        protected override void OnBeforeGenerate()
        {
            Assert.IsTrue(this.iterations > 0);
            Assert.IsNotNull(this.rules);
            Assert.IsTrue(this.rules.Count > 0);
        }

        protected override IEnumerable<Grid2D<int>> Modify(Grid2D<int> grid)
        {
            var buffer = grid.CopyShape<int>();

            var automata = new CellularAutomata<int>(this.rules);
            for (int i = 0; i < this.iterations; i++)
            {
                automata.Step(grid, buffer);
                (buffer, grid) = (grid, buffer);

                yield return grid;
            }
        }

        [CustomPortBehavior(nameof(rules))]
        IEnumerable<PortData> GetPortsForInputs(List<SerializableEdge> edges)
        {
            yield return new PortData
            {
                displayName = nameof(this.rules),
                displayType = typeof(ICellularRule<int>),
                acceptMultipleEdges = true
            };
        }

        [CustomPortInput(nameof(rules), typeof(ICellularRule<int>), allowCast = true)]
        public void GetInputs(List<SerializableEdge> edges)
        {
            rules = edges.Select(e => (ICellularRule<int>)e.passThroughBuffer).ToList();
        }
    }
}