using System;
using System.Collections.Generic;
using Chinchillada.CellularAutomata;
using Chinchillada.PCG.Grid;
using Chinchillada.Grid;
using GraphProcessor;
using UnityEngine;

namespace Chinchillada.Grid.PCGraphs
{
    [Serializable, NodeMenuItem("Ints/Grids/CA1D")]
    public class CA1DNode : IntGridGeneratorNode
    {
        [SerializeField, Input] private int rowCount;

        [SerializeField] private List<int> firstRow;
        
        [SerializeField, Input] private IntCellularAutomata1D automaton;

        protected override IEnumerable<Grid2D<int>> GenerateAsync()
        {
            var builder = new CA1DGridBuilder(this.automaton, this.firstRow, this.rowCount);
            return builder.BuildIterative();
        }
    }
}