using System;
using System.Collections.Generic;
using Chinchillada;
using Chinchillada.Grid;
using UnityEngine;

namespace Chinchillada.PCG.Grid
{
    [Serializable]
    public class CellularAutomata
    {
        [SerializeReference] private List<ICellularRule> rules = new();

        public CellularAutomata()
        {
        }

        public CellularAutomata(List<ICellularRule> rules)
        {
            this.rules = rules;
        }
        
        public void Step(Grid2D<int> input, Grid2D<int> output)
        {
            for (var x = 0; x < input.Width; x++)
            for (var y = 0; y < input.Height; y++)
                output[x, y] = this.ApplyRules(x, y, input);
        }

        private int ApplyRules(int x, int y, Grid2D<int> grid)
        {
            var value = grid[x, y];

            for (var i = this.rules.Count - 1; i >= 0; i--)
            {
                var rule = this.rules[i];
                var newValue = rule.Apply(x, y, grid);

                if (newValue != value)
                    return newValue;
            }

            return value;
        }
    }
}