using System;
using System.Collections.Generic;
using System.Linq;
using Foundation.Algorithms;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Chinchillada.CellularAutomata
{
    [Serializable]
    public class CellularAutomata1D<T> where T : IEquatable<T>
    {
        [SerializeField] private List<Rule> rules = new List<Rule>();

        public List<Rule> Rules => this.rules;

        public IList<T> Apply(IList<T> input)
        {
            var output = new T[input.Count];

            this.Apply(input, output);

            return output;
        }

        public void ApplyInPlace(IList<T> input) => this.Apply(input, input);

        public void Apply(IList<T> input, IList<T> output)
        {
            for (var index = 1; index < input.Count - 1; index++)
            {
                foreach (var rule in this.Rules)
                {
                    if (rule.TryApply(index, input, output))
                        break;
                }
            }
        }

        public Rule FindMatchingRule(IList<T> input, int index) => this.rules.First(rule => rule.Match(index, input));

        [Button, FoldoutGroup("Edit")]
        protected void InitializeRulePermutations(IList<T> states)
        {
            var permutationRules = GenerateRules();
            var newRules = permutationRules.Where(IsNew);

            foreach (var rule in newRules.ToList())
                this.Rules.Add(rule);

            IEnumerable<Rule> GenerateRules()
            {
                var permutations = Permutations.Generate(states, 3);
                foreach (var permutation in permutations)
                    yield return new Rule(permutation[0], permutation[1], permutation[2]);
            }

            bool IsNew(Rule rule) => !this.Rules.Any(currentRule => currentRule.HasSamePattern(rule));
        }

        [Serializable]
        public class Rule
        {
            private const string PatternGroup = "Pattern";

            [SerializeField, HideLabel, HorizontalGroup(PatternGroup)]
            private T leftNeighbor;

            [SerializeField, HideLabel, HorizontalGroup(PatternGroup)]
            private T cell;

            [SerializeField, HideLabel, HorizontalGroup(PatternGroup)]
            private T rightNeighbor;

            [SerializeField] private T result;

            public Rule()
            {
            }

            public Rule(T leftNeighbor, T cell, T rightNeighbor)
            {
                this.leftNeighbor = leftNeighbor;
                this.cell = cell;
                this.rightNeighbor = rightNeighbor;
            }

            public T Result
            {
                get => this.result;
                set => this.result = value;
            }

            public T LeftNeighbor => this.leftNeighbor;

            public T Cell => this.cell;

            public T RightNeighbor => this.rightNeighbor;

            public bool HasSamePattern(Rule other)
            {
                return other.LeftNeighbor.Equals(this.LeftNeighbor) &&
                       other.Cell.Equals(this.Cell) &&
                       other.RightNeighbor.Equals(this.RightNeighbor);
            }

            public bool TryApply(int index, IList<T> input, IList<T> output)
            {
                if (!this.Match(index, input))
                    return false;

                this.Apply(index, output);
                return true;
            }

            public void Apply(int index, IList<T> output) => output[index] = this.Result;

            public bool Match(int index, IList<T> input)
            {
                return MatchCell(index - 1, this.LeftNeighbor) &&
                       MatchCell(index, this.Cell) &&
                       MatchCell(index + 1, this.RightNeighbor);

                bool MatchCell(int i, T item)
                {
                    var inputCell = input[i];
                    return inputCell.Equals(item);
                }
            }
        }
    }
}