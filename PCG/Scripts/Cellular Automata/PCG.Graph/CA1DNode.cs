using System.Collections.Generic;
using Chinchillada.CellularAutomata;
using Chinchillada.Generation.Grid;
using Chinchillada.Grid;
using UnityEngine;

namespace Chinchillada.GeneratorGraph.Grid
{
    public class CA1DNode : GridGeneratorNode
    {
        [SerializeField, Input] private int rowCount;

        [SerializeField, Input] private List<int> firstRow;
        
        [SerializeField] private CellularAutomata1D<int> automaton;

        protected override void UpdateInputs()
        {
            this.rowCount = this.GetInputValue(nameof(this.rowCount), this.rowCount);
            this.firstRow = this.GetInputValue(nameof(this.firstRow), this.firstRow);
        }

        protected override Grid2D GenerateGrid()
        {
            return CA1DGridGenerator.GenerateGrid(this.rowCount, this.firstRow, this.automaton);
        }
    }
}