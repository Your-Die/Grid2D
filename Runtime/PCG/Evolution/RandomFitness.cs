using Chinchillada.Grid;
using UnityEngine;

namespace Chinchillada.PCG.Evolution.Grid
{
    public class RandomFitness : MonoBehaviour, IMetricEvaluator<Grid2D<int>>
    {
        [SerializeField] private Vector2 fitnessRange;
        public float Evaluate(Grid2D<int> item)
        {
            return this.fitnessRange.RandomInRange();
        }
    }
}