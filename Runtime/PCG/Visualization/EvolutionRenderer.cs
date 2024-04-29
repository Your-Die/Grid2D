using System.Collections;
using Chinchillada;
using Chinchillada.Grid.Visualization;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Chinchillada.PCG.Evolution.Grid
{
    public class EvolutionRenderer : AutoRefBehaviour
    {
        [SerializeField] private float updateInterval = 0.01f;
        
        [SerializeField, FindComponent(SearchStrategy.InScene)]
        private GridEvolution evolution;
        
        [SerializeReference, FindComponent(SearchStrategy.InScene)]
        private IIntGridRenderer gridRenderer;

        [SerializeReference] private IRNG random;

        private IEnumerator routine;

        [Button]
        public void RunEvolution()
        {
            if (this.routine != null) 
                this.StopCoroutine(this.routine);

            this.routine = this.EvolutionRoutine();
            this.StartCoroutine(this.routine);
        }

        private IEnumerator EvolutionRoutine()
        {
            foreach (var genotype in this.evolution.EvolveGenerationWise(random))
            {
                var grid = genotype.Candidate;
                Debug.Log("Best Fitness " + genotype.Fitness);
                
                this.gridRenderer.Render(grid);
                
                yield return new WaitForSeconds(this.updateInterval);
            }
        }
    }
}