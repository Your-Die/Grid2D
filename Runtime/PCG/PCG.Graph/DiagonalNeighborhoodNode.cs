using System;
using Chinchillada.PCGraphs;
using GraphProcessor;

namespace Chinchillada.Grid.PCGraphs
{
    [Serializable, NodeMenuItem("Grid/Neighborhood/Diagonal")]
    public class DiagonalNeighborhoodNode : GeneratorNode<Diagonal.Factory>
    {
        public override Diagonal.Factory Generate()
        {
            return new Diagonal.Factory();
        }
    }
    
    [Serializable, NodeMenuItem("Grid/Neighborhood/Orthogonal")]
    public class OrthogonalNeighborhoodNode : GeneratorNode<Orthogonal.Factory>
    {
        public override Orthogonal.Factory Generate()
        {
            return new Orthogonal.Factory();
        }
    }
}