﻿using System;
using System.Collections.Generic;
using GraphProcessor;
using UnityEngine;

namespace Chinchillada.Grid.PCGraphs
{
    [Serializable, NodeMenuItem("Grid/Int/Uniform")]
    public class UniformGridNode : IntGridGeneratorNode, IUsesRNG
    {
        [SerializeField, Input, ShowAsDrawer] private int width;
        [SerializeField, Input, ShowAsDrawer] private int height;

        [SerializeField, Input, ShowAsDrawer] private int value;
        public IRNG RNG { get; set; }

        protected override IEnumerable<Grid2D<int>> GenerateAsync()
        {
            yield return new Grid2D<int>(this.width, this.height, this.value);
        }
    }
}