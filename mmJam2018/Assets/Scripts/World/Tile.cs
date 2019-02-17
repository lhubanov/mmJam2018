using UnityEngine;

using FMODUnity;
using FMOD.Studio;

using Assets.Scripts;

public class Tile : MonoBehaviour, IDie, IHoldEnergy
{
    [SerializeField]
    private bool isDead = false;

    [SerializeField]
    private TileLookup deadTileLookup;

    [SerializeField]
    private float heldEnergy;


    public float GetHeldEnergy()
    {
        return heldEnergy;
    }

    public void Die()
    {
        if (!isDead) {
            DrainTile(transform);
            heldEnergy = 0;
            isDead = true;
        }
    }

    private void DrainTile(Transform transform)
    {
        foreach (Transform t in transform)
        {
            if (t.GetComponent<SpriteRenderer>() != null)
            {
                Sprite deadSprite;
                string spriteName = t.GetComponentInChildren<SpriteRenderer>().sprite.name;

                if (deadTileLookup.DeadSprites.TryGetValue(spriteName, out deadSprite)) {
                    t.GetComponentInChildren<SpriteRenderer>().sprite = deadSprite;
                }
            }

            if (t.childCount > 0) {
                DrainTile(t);
            }
        }
    }
}
