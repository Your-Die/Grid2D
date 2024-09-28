using System;
using Chinchillada.PCGraphs;
using GraphProcessor;

namespace Chinchillada.Grid.PCGraphs
{
    [Serializable, NodeMenuItem("Grid/Neighborhood/Orthogonal")]
    public class OrthogonalNeighborhoodNode : GeneratorNode<Orthogonal.Factory>
    {
        [ShowInInspector] public int radius = 1;

        [ShowInInspector] public bool includeCenter = true;
        
        public override Orthogonal.Factory Generate()
        {
            return new Orthogonal.Factory(this.radius, this.includeCenter);
        }
    }
}