using System;
using System.Linq;
using Chinchillada.Grid;
using JetBrains.Annotations;
using UnityEngine;

namespace Chinchillada.PCG.Evolution.Grid
{
    [Serializable, UsedImplicitly]
    public class CountFitness : IMetricEvaluator<Grid2D<int>>
    {
        [SerializeField] private int targetType;

        public float Evaluate(Grid2D<int> grid)
        {
            var targetCount = grid.Count(item => item == this.targetType);
            return (float) targetCount / grid.Size;
        }
    }
}