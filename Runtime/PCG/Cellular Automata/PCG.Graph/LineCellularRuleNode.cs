using System;
using Chinchillada.PCGraphs;
using GraphProcessor;

namespace Chinchillada.Grid
{
    [Serializable, NodeMenuItem("Grid/Int/Cellular Automata/Line Rule")]
    public class LineCellularRuleNode : GeneratorNode<LineCellularRule<int>>
    {
        [ShowInInspector] public int[] match;

        [Input, ShowAsDrawer] public int result;
        
        public override LineCellularRule<int> Generate() => new(this.match, this.result);
    }
}