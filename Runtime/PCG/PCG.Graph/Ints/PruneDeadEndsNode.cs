using System;
using System.Collections.Generic;
using System.Linq;
using GraphProcessor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Chinchillada.Grid.PCGraphs
{
    [Serializable, NodeMenuItem("Grid/Int/Prune Dead Ends")]
    public class PruneDeadEndsNode : IntGridModifierNode
    {
        [Input, ShowAsDrawer] public int prunableValue;
        
        [Input, ShowAsDrawer] public int emptyValue;

        [Input] public INeighborhoodFactory neighborhoodFactory;

        [FormerlySerializedAs("neighborCount")] [Input, ShowAsDrawer] public int minNeighborCount = 2;
        
        public override int ExpectedIterations => Mathf.Max(this.inputGrid.Width, this.inputGrid.Height);

        protected override IEnumerable<Grid2D<int>> Modify(Grid2D<int> grid)
        {
            var neighborAccountant = CellNeighborAccountant<int>.CountNeighbors(grid, this.neighborhoodFactory, this.emptyValue);

            yield return grid;

            bool hasPruned;
            do
            {
                hasPruned = false;
                
                var cellsToPrune = neighborAccountant.GetCellsWhere(neighborCount => neighborCount < this.minNeighborCount)
                                                     .Where(cell => grid[cell] == this.prunableValue);

                foreach (var cell in cellsToPrune)
                {
                    Prune(cell);

                    yield return grid;
                    hasPruned = true;
                }

            } while (hasPruned);

            void Prune(Vector2Int cell)
            {
                grid[cell] = this.emptyValue;
                neighborAccountant.RemoveAt(cell);
            }
        }
        

        private class CellNeighborAccountant<T>
        {
            private readonly Grid2D<int>          neighborCounts;
            private readonly INeighborhoodFactory neighborhoodFactory;

            private CellNeighborAccountant(Grid2D<int> neighborCounts, INeighborhoodFactory factory)
            {
                this.neighborCounts = neighborCounts;
                this.neighborhoodFactory = factory;
            }

            public static CellNeighborAccountant<T> CountNeighbors(Grid2D<T> grid,
                                                                   INeighborhoodFactory neighborhoodFactory,
                                                                   T emptyValue = default)
            {
                var neighborCounts = grid.CopyShape<int>();

                foreach (Vector2Int coordinate in grid.Coordinates)
                {
                    GridNeighborhood neighborhood = neighborhoodFactory.Get(grid, coordinate);
                    neighborCounts[coordinate] = neighborhood.Count(IsFilled);
                }

                return new CellNeighborAccountant<T>(neighborCounts, neighborhoodFactory);

                bool IsFilled(Vector2Int cell)
                {
                    T value = grid[cell];
                    return !value.Equals(emptyValue);
                }
            }

            public IEnumerable<Vector2Int> GetCellsWhere(Func<int, bool> selector)
            {
                return this.neighborCounts.Coordinates.Where(coordinate => selector.Invoke(this.neighborCounts[coordinate]));
            }

            public void RemoveAt(Vector2Int cell)
            {
                GridNeighborhood neighbors = this.neighborhoodFactory.Get(this.neighborCounts, cell);
                
                foreach (Vector2Int neighbor in neighbors) 
                    this.neighborCounts[neighbor]--;
            }
        }
    }
}