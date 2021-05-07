using Chinchillada;
using Sirenix.OdinInspector;
using UnityEngine.Assertions;

namespace Chinchillada.CellularAutomata
{
    public class ElementaryCellularAutomaton : CellularAutomata1D<int>
    {
        [Button, FoldoutGroup("Edit")]
        public void SetWolframRuleSet(byte number)
        {
            var bitArray = number.ToBitArray();

            this.Rules.Clear();
            InitializePermutations(0, 1);

            Assert.IsTrue(this.Rules.Count == bitArray.Count);

            for (var index = 0; index < this.Rules.Count; index++)
            {
                var rule = this.Rules[index];
                var bit = bitArray[index];
                
                rule.Result = bit.ToBinary();
            }

            void InitializePermutations(params int[] states) => this.InitializeRulePermutations(states);
        }
    }
}