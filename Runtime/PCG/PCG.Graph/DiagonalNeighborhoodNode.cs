using System;
using Chinchillada.PCGraphs;
using GraphProcessor;

namespace Chinchillada.Grid.PCGraphs
{
    [Serializable, NodeMenuItem("Grid/Neighborhood/Diagonal")]
    public class DiagonalNeighborhoodNode : GeneratorNode<Diagonal.Factory>
    {
        [ShowInInspector] public int radius = 1;

        [ShowInInspector] public bool includeCenter = true;
        
        public override Diagonal.Factory Generate()
        {
            return new Diagonal.Factory(this.radius, this.includeCenter);
        }
    }
}