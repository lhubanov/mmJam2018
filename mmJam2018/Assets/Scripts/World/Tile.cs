using UnityEngine;

using FMODUnity;
using FMOD.Studio;

using Assets.Scripts;

public class Tile : MonoBehaviour, IDie, IHoldEnergy
{
    // FIXME: Run analysis to find all unreferenced properties
    [EventRef]
    public string DrainSound;
    private EventInstance drainInstance;

    [SerializeField]
    private bool isDead = false;

    [SerializeField]
    private TileLookup deadTileLookup;

    [SerializeField]
    private float heldEnergy;

    // All to do with the way the spritesheet gets broken down into child sprites;
    // Need to figure out a different way to get these at run time; this string parsing is not great
    private int GetResourceID(string spriteName)
    {
        char resourceID = spriteName[spriteName.Length - 1];
        return (char.GetNumericValue(resourceID) != -1) ? (int)char.GetNumericValue(resourceID) : 0;
    }

    public float GetHeldEnergy()
    {
        return heldEnergy;
    }

    public void Die()
    {
        if (!isDead) {
            DrainTile(transform);
            isDead = true;
        }
    }

    // FIXME: Do the Sprite tango here, when the lookup is refactored
    private void DrainTile(Transform transform)
    {
        foreach (Transform t in transform)
        {
            if (t.GetComponent<SpriteRenderer>() != null)
            {
                string spriteSheetName;
                string spriteName = t.GetComponentInChildren<SpriteRenderer>().sprite.name;
                if (deadTileLookup.DeadSprites.TryGetValue(spriteName, out spriteSheetName))
                {
                    Sprite[] resources = Resources.LoadAll<Sprite>(spriteSheetName);
                    t.GetComponentInChildren<SpriteRenderer>().sprite = resources[GetResourceID(spriteName)];
                }
            }

            if (t.childCount > 0) {
                DrainTile(t);
            }
        }
    }
}
