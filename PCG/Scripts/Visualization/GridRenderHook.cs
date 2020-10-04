using System.Collections;
using Chinchillada.Foundation;
using Chinchillada.Grid;
using Chinchillada.Grid.Visualization;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Chinchillada.Generation.Grid
{
    using UnityEngine.Events;

    public class GridRenderHook : ChinchilladaBehaviour
    {
        [SerializeField,
         FindComponent(SearchStrategy.InChildren),
         OnValueChanged(nameof(UpdateGenerator))]
        private IIterativeGenerator<Grid2D> generator;

        [SerializeField, FindComponent(SearchStrategy.InChildren)]
        private IGridRenderer drawer;

        private IIterativeGenerator<Grid2D> generatorCache;

        private IEnumerator routine;

        public UnityEvent BeforeRender;

        [Button]
        public void InvokeGeneration() => this.generator.Generate();

        [Button]
        public void InvokeGenerationAsync()
        {
            if (this.routine != null)
                this.StopCoroutine(this.routine);

            this.routine = this.generator.GenerateAsyncRoutine(this.Render);
            this.StartCoroutine(this.routine);
        }

        [Button]
        public void StartGenerationAsync()
        {
            if (this.routine != null)
                this.StopCoroutine(this.routine);

            this.routine = this.generator.GenerateAsyncRoutine(this.Render);
        }

        [Button]
        public void StepAsyncGeneration()
        {
            if (this.routine == null || !this.routine.MoveNext())
                return;

            var grid = this.generator.Result;
            this.Render(grid);
        }

        private void Render(Grid2D grid)
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

        private void Subscribe(IIterativeGenerator<Grid2D> gridGenerator)
        {
            gridGenerator.Generated += this.Render;
        }

        private void Unsubscribe(IIterativeGenerator<Grid2D> gridGenerator)
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