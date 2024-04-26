using System.Collections;
using Chinchillada;
using Chinchillada.Grid;
using Chinchillada.Grid.Visualization;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Chinchillada.PCG.Grid
{
    using UnityEngine.Events;

    public class GridRenderHook : AutoRefBehaviour
    {
        [SerializeField,
         FindComponent(SearchStrategy.InChildren),
         OnValueChanged(nameof(UpdateGenerator))]
        private IAsyncGenerator<Grid2D<int>> generator;

        [SerializeField, FindComponent(SearchStrategy.InChildren)]
        private IIntGridRenderer drawer;

        [SerializeReference] private IRNG random;

        private IAsyncGenerator<Grid2D<int>> generatorCache;

        private IEnumerator routine;

        public UnityEvent BeforeRender;

        [Button]
        public void InvokeGeneration() => this.generator.Generate(random);

        [Button]
        public void InvokeGenerationAsync()
        {
            if (this.routine != null)
                this.StopCoroutine(this.routine);

            this.routine = this.generator.GenerateAsyncRoutine(random, this.Render);
            this.StartCoroutine(this.routine);
        }

        [Button]
        public void StartGenerationAsync()
        {
            if (this.routine != null)
                this.StopCoroutine(this.routine);

            this.routine = this.generator.GenerateAsyncRoutine(this.random, this.Render);
        }

        [Button]
        public void StepAsyncGeneration()
        {
            if (this.routine == null || !this.routine.MoveNext())
                return;

            var grid = this.generator.Result;
            this.Render(grid);
        }

        private void Render(Grid2D<int> grid)
        {
            this.BeforeRender.Invoke();
            this.drawer.Render(grid);
        }

        private void UpdateGenerator()
        {
            if (this.generator == this.generatorCache)
                return;

            Unsubscribe(this.generatorCache);
            Subscribe(this.generator);

            this.generatorCache = this.generator;
        }

        private void Subscribe(IAsyncGenerator<Grid2D<int>> gridGenerator)
        {
            gridGenerator.Generated += this.Render;
        }

        private void Unsubscribe(IAsyncGenerator<Grid2D<int>> gridGenerator)
        {
            if (gridGenerator != null)
                gridGenerator.Generated -= this.Render;
        }

        protected override void Awake()
        {
            base.Awake();
            this.generatorCache = this.generator;
        }

        private void OnEnable() => Subscribe(this.generator);

        private void OnDisable() => Unsubscribe(this.generator);
    }
}