using System;
using System.Collections.Generic;
using Chinchillada.CellularAutomata;
using Chinchillada.Common;
using Chinchillada.Grid;
using UnityEngine;

namespace Chinchillada.PCG.Grid
{
    [Serializable]
    public class CA1DGridBuilder : IIterativeBuilder<Grid2D<int>>
    {
        [SerializeField] private IntCellularAutomata1D automaton;

        [SerializeField] private List<int> firstRow;

        [SerializeField] private int rowCount;

        public CA1DGridBuilder(IntCellularAutomata1D automaton, List<int> firstRow, int rowCount)
        {
            this.automaton = automaton;
            this.firstRow = firstRow;
            this.rowCount = rowCount;
        }

        public IEnumerable<Grid2D<int>> BuildIterative()
        {
            var grid = InitializeGrid(this.firstRow, this.rowCount);

            yield return grid;

            var previousRow = new GridRowAccessor(grid);
            var row = new GridRowAccessor(grid);

            for (row.RowIndex = 1; row.RowIndex < grid.Height; row.RowIndex++)
            {
                this.automaton.Apply(previousRow, row);
                yield return grid;

                previousRow.RowIndex = row.RowIndex;
            }
        }

        public static Grid2D<int> GenerateGrid(int rowCount, IList<int> firstRow, CellularAutomata1D<int> automaton)
        {
            var grid = InitializeGrid(firstRow, rowCount);

            var previousRow = new GridRowAccessor(grid);
            var row = new GridRowAccessor(grid);

            for (row.RowIndex = 1; row.RowIndex < grid.Height; row.RowIndex++)
            {
                automaton.Apply(previousRow, row);
                previousRow.RowIndex = row.RowIndex;
            }

            return grid;
        }

        private static Grid2D<int> InitializeGrid(IList<int> firstRow, int rowCount)
        {
            var width = firstRow.Count;

            var grid = new Grid2D<int>(width, rowCount + 1);

            for (var x = 0; x < firstRow.Count; x++)
                grid[x, 0] = firstRow[x];

            return grid;
        }
    }
}