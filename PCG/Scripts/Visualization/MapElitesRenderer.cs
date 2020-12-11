using Chinchillada.Generation.Evolution;
using Chinchillada.Foundation;
using Chinchillada.Grid;
using Chinchillada.Grid.Visualization;
using Sirenix.OdinInspector;
using UnityEngine;

public class MapElitesRenderer : ChinchilladaBehaviour
{
    [SerializeField, FindComponent(SearchStrategy.InScene), Required]
    private MapElites<Grid2D> mapElites;

    [SerializeField, Required] private GridImageRenderer rendererPrefab;

    private GridImageRenderer[,] renderers;

    private void OnEnable() => this.mapElites.PopulationChanged += this.UpdateDisplay;
    private void OnDisable() => this.mapElites.PopulationChanged -= this.UpdateDisplay;

    private void UpdateDisplay()
    {
        this.SetupGrid(this.mapElites.Width, this.mapElites.Height);

        for (var x = 0; x < this.mapElites.Width; x++)
        for (var y = 0; y < this.mapElites.Height; y++)
        {
            var genotype = this.mapElites.Map[x, y];
            
            if (genotype != null)
                this.renderers[x, y].Render(genotype.Candidate);
            else
                this.renderers[x, y].enabled = false;
        }
    }

    private void SetupGrid(int width, int height)
    {
        if (this.renderers != null)
        {
            for (int x = 0; x < this.renderers.GetLength(0); x++)
            for (int y = 0; y < this.renderers.GetLength(1); y++)
                Destroy(this.renderers[x, y].gameObject);
        }

        this.renderers = new GridImageRenderer[width, height];
        for (var x = 0; x < width; x++)
        for (var y = 0; y < height; y++)
            this.renderers[x, y] = Instantiate(this.rendererPrefab, this.transform);
    }
}