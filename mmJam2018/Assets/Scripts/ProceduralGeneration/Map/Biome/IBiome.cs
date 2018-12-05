using ProceduralGeneration.Map;

namespace ProceduralGeneration.Biome
{
    public interface IBiome
    {
        // DEBUG: Temporary, only due to text file visualisation. Remove in Unity
        BiomeType biomeType { get; }

        bool HasSpawned { get; }

        // A bunch of probabilities go here, 
        // which, for now, can be private class members;
        // Add if found necessary.

        // Returns bool if anything got spawned, so that multiple
        // items don't get spawned on top of each other on a single tile
        bool SpawnMembers(Center tile);
    }
}
