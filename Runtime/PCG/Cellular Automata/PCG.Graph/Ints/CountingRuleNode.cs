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

        [Input] public INeighborhoodFactory neighborhood = new Diagonal.Factory();
        
        [ShowInInspector] public CountConstraint Constraint;
        
        public override CountingRule<int> Generate()
        {
            return new CountingRule<int>(this.resultValue, this.targetValue, this.Constraint, this.neighborhood);
        }
    }
}