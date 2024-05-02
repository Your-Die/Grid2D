using System;
using Chinchillada.PCG.Grid;
using Chinchillada.PCGraphs;
using GraphProcessor;

namespace Chinchillada.Grid.PCGraphs.Ints
{
    [Serializable, NodeMenuItem("Grid/Int/Cellular Automata/Counting Rule")]
    public class CountingRuleNode : GeneratorNode<CountingRule<int>>
    {
        [Input, ShowAsDrawer] public int resultValue;
        [Input, ShowAsDrawer] public int targetValue;

        [Input, ShowAsDrawer] public int constraintValue = 3;
        [Input, ShowAsDrawer] public CountConstraint.ComparisonOperator constraintOperator;

        [Input] public INeighborhoodFactory neighborhood = new Diagonal.Factory();

        public override CountingRule<int> Generate()
        {
            var constraint = new CountConstraint(this.constraintOperator, this.constraintValue);
            return new CountingRule<int>(this.resultValue, this.targetValue, constraint, this.neighborhood);
        }
    }
}