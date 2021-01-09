using System;
using System.Collections.Generic;
using Chinchillada.CellularAutomata;
using Chinchillada.Grid;
using UnityEngine;

namespace Chinchillada.Generation.Grid
{
    [Serializable]
    public class CA1DGridGenerator : AsyncGeneratorBase<Grid2D>
    {
        [SerializeField] private CellularAutomata1D<int> automaton;

        [SerializeField] private IGenerator<IList<int>> firstRowGenerator;

        [SerializeField] private int rowCount;

        public override IEnumerable<Grid2D> GenerateAsync()
        {
            var firstRow = this.firstRowGenerator.Generate();
            var grid = InitializeGrid(firstRow, this.rowCount);
            
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

        public static Grid2D GenerateGrid(int rowCount, IList<int> firstRow, CellularAutomata1D<int> automaton)
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

        private static Grid2D InitializeGrid(IList<int> firstRow, int rowCount)
        {
            var width = firstRow.Count;

            var grid = new Grid2D(width, rowCount + 1);

            for (var x = 0; x < firstRow.Count; x++)
                grid[x, 0] = firstRow[x];

            return grid;
        }
    }
}