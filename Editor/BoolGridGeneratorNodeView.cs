namespace Chinchillada.PCGraphs.Editor
{
    using System.Collections.Generic;
    using GraphProcessor;
    using JetBrains.Annotations;
    using Grid.PCGraphs;
    using UnityEngine;

    [UsedImplicitly, NodeCustomEditor(typeof(BoolGridGeneratorNode))]
    public class BoolGridGeneratorNodeView : GridGeneratorNodeView<BoolGridGeneratorNode, bool>
    {
        private static readonly Dictionary<bool, Color> ColorsByValue = new Dictionary<bool, Color>
        {
            { false, Color.white },
            { true, Color.black }
        };
        
        protected override Color PickColor(bool value)
        {
            return ColorsByValue[value];
        }
    }
}