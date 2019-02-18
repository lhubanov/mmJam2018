using UnityEngine;
using Assets.Scripts;

using ProceduralGeneration.Graph;
using ProceduralGeneration.Map;
using ProceduralGeneration.Map.MapSettings;

public class RegionGenerator : MonoBehaviour
{
    [SerializeField]
    private Transform    RegionTopLeft;

    [SerializeField]
    private Transform    RegionBotRight;

    [SerializeField]
    private TileLookup   TileLookup;

    [SerializeField]
    private BiomeConditions BiomeConditions;

    [SerializeField]
    private bool         UseRandomSeed;

    [SerializeField]
    private string       Seed;

    // Note: This assumes a tile is square;
    [SerializeField]
    private float tileSize;

    [Range(0, 1)]
    [SerializeField]
    private float oceanThreshold = 0.85f;

    [Range(0, 1)]
    [SerializeField]
    private float chanceIslandTileIsWater = 0.5f;

    [SerializeField]
    private double elevationIncreaseRate = 0.95;

    [SerializeField]
    private int spawningIterations = 2;

    [SerializeField]
    private float noiseScale = 1f;

    [SerializeField]
    private int maxNoiseOffset = 100;

    [SerializeField]
    private GRAPH_TRAVERSAL_METHOD graphTraversalMethod = GRAPH_TRAVERSAL_METHOD.BFS;

    private Map map = null;

    private void Start()
    {
        GenerateRegion();
	}

    private void GenerateRegion()
    {
        if (UseRandomSeed) {
            Seed = Random.Range(0, 1000).ToString();
        }

        System.Random randomNumberGenerator = new System.Random(Seed.GetHashCode());

        MapSettingsContainer settings = PackageSettings();
        map = new Map(settings);
        map.Generate();
    }

    private void Update()
    {
        //if (Input.GetMouseButton(1))
        //{
        //    DestroyAllTiles();
        //    GenerateRegion();
        //}
    }

    private MapSettingsContainer PackageSettings()
    {
        MapSettingsContainer settings = new MapSettingsContainer(
            new Vector2(RegionTopLeft.position.x, RegionBotRight.position.y),
            new Vector2(RegionBotRight.position.x, RegionTopLeft.position.y),
            tileSize,
            oceanThreshold,
            chanceIslandTileIsWater,
            elevationIncreaseRate,
            spawningIterations,
            noiseScale,
            maxNoiseOffset,
            Seed,
            TileLookup,
            BiomeConditions,
            transform,
            graphTraversalMethod
        );

        return settings;
    }

    private void DestroyAllTiles()
    {
        foreach(Transform child in transform)
        {
            if (child.GetComponent<Tile>() != null ||
                child.GetComponent<SlowDownPlayer>() != null)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
