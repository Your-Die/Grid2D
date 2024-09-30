using System;
using Chinchillada.PCG.Grid;
using UnityEngine;

namespace Chinchillada.Grid
{
    [Serializable]
    public class LineCellularRule<T> : ICellularRule<T>
    {
        [SerializeField] private T[] match;

        [SerializeField] private T result;

        public T Result => this.result;

        public LineCellularRule(T[] match, T result)
        {
            this.match = match;
            this.result = result;
        }

        public bool Match(int xCenter, int y, Grid2D<T> grid, out T resultValue)
        {
            resultValue = this.result;
            
            int left = Mathf.FloorToInt(xCenter - this.match.Length / 2f);

            for (var i = 0; i < this.match.Length; i++)
            {
                int x = left + i;
                
                T expected = this.match[i];
                T actual = grid.Contains(x, y) ? grid[x, y] : default;

                if (!Equals(expected, actual))
                    return false;
            }

            return true;
        }

        public override string ToString() => $"[{this.match.JoinWithComma()}] -> {this.result}";
    }
}