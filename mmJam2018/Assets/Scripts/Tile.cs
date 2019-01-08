using UnityEngine;

using FMODUnity;
using FMOD.Studio;

using Assets.Scripts;

public class Tile : MonoBehaviour
{
    // FIXME: Run analysis to find all unreferenced properties
    [EventRef]
    public string DrainSound;
    private EventInstance drainInstance;

    private bool isDead = false;
    public TileLookup DeadTileLookup;

    // All to do with the way the spritesheet gets broken down into child sprites;
    // Need to figure out a different way to get these at run time; this string parsing is not great
    private int GetResourceID(string spriteName)
    {
        char resourceID = spriteName[spriteName.Length - 1];
        return (char.GetNumericValue(resourceID) != -1) ? (int)char.GetNumericValue(resourceID) : 0;
    }

    public void DrainTile()
    {
        if (!isDead) {
            DrainTile(transform);
            isDead = true;
        }
    }

    private void DrainTile(Transform transform)
    {
        foreach (Transform t in transform)
        {
            if (t.GetComponent<SpriteRenderer>() != null)
            {
                string spriteSheetName;
                string spriteName = t.GetComponentInChildren<SpriteRenderer>().sprite.name;
                if (DeadTileLookup.DeadSprites.TryGetValue(spriteName, out spriteSheetName))
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
