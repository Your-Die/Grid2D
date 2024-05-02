using System;
using System.Collections.Generic;
using System.ComponentModel;
using Chinchillada.Grid;
using UnityEngine;

namespace Chinchillada.PCG.Grid
{
    [Serializable]
    public class CellularAutomata<T>
    {
        [SerializeReference] private List<ICellularRule<T>> rules = new();

        public CellularAutomata()
        {
        }

        public CellularAutomata(List<ICellularRule<T>> rules)
        {
            this.rules = rules;
        }
        
        public void Step(Grid2D<T> input, Grid2D<T> output)
        {
            for (int x = 0; x < input.Width; x++)
            for (int y = 0; y < input.Height; y++)
                output[x, y] = this.ApplyRules(x, y, input);
        }

        private T ApplyRules(int x, int y, Grid2D<T> grid)
        {
            T value = grid[x, y];

            for (var index = this.rules.Count - 1; index >= 0; index--)
            {
                var rule = this.rules[index];

                if (rule.Match(x, y, grid, out var newValue))
                    return newValue;
            }

            return value;
        }
    }
}