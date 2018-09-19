﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using FMODUnity;
using FMOD.Studio;

using Assets.Scripts;

public class Tile : MonoBehaviour
{
    [EventRef]
    public string DrainSound;
    private EventInstance drainInstance;

    private bool isDead = false;
    public DeadTileLookup DeadTileLookup;

    void Start()
    {

    }

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
                if (DeadTileLookup.deadSprites.TryGetValue(spriteName, out spriteSheetName))
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
