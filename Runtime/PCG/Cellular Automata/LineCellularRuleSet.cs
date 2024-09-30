using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chinchillada.Grid
{
    [Serializable]
    public class LineCellularRuleSet<T>
    {
        [SerializeField] private T defaultResult;

        [SerializeField] private List<LineCellularRule<T>> rules;

        public LineCellularRuleSet(IEnumerable<LineCellularRule<T>> rules)
        {
            this.rules = rules.ToList();
        }

        public void GenerateLineAtHeight(Grid2D<T> grid, int height)
        {
            int previous = height - 1;

            if (previous < 0 || height >= grid.Height)
                throw new ArgumentException();

            for (var x = 0; x < grid.Width; x++)
            {
                grid[x, height] = this.MatchValueAt(x, previous, grid);
            }
        }

        private T MatchValueAt(int x, int y, Grid2D<T> grid)
        {
            foreach (var rule in this.rules)
            {
                if (rule.Match(x,y, grid, out var result))
                {
                    return result;
                }
            }

            return this.defaultResult;
        }

        public override string ToString() => this.rules.JoinWithNewLine();
    }
}