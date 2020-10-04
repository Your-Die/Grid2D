using System;
using System.Linq;
using Chinchillada.Grid;
using JetBrains.Annotations;
using UnityEngine;

namespace Chinchillada.Generation.Evolution.Grid
{
    [Serializable, UsedImplicitly]
    public class CountFitness : IMetricEvaluator<Grid2D>
    {
        [SerializeField] private int targetType;

        public float Evaluate(Grid2D grid)
        {
            var targetCount = grid.Count(item => item == this.targetType);
            return (float) targetCount / grid.Size;
        }
    }
}