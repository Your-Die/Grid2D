using System;
using UnityEngine;

namespace Chinchillada.Generation.Grid
{
    [Serializable]
    public class CountConstraint
    {
        [SerializeField] private ComparisonOperator constraintOperator;
        
        [Header("Constraint")] [SerializeField]
        private int constraintAmount = 3;

        
        /// <summary>
        /// Validates the constraint.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public bool ValidateConstraint(int count)
        {
            switch (this.constraintOperator)
            {
                case ComparisonOperator.LesserThan:
                    return count < this.constraintAmount;

                case ComparisonOperator.GreaterThan:
                    return count > this.constraintAmount;

                case ComparisonOperator.Equal:
                    return count == this.constraintAmount;

                case ComparisonOperator.LesserOrEqual:
                    return count <= this.constraintAmount;

                case ComparisonOperator.GreaterOrEqual:
                    return count >= this.constraintAmount;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        /// <summary>
        /// Comparison operators.
        /// </summary>
        private enum ComparisonOperator
        {
            LesserThan,
            GreaterThan,
            Equal,
            LesserOrEqual,
            GreaterOrEqual
        }
    }
}