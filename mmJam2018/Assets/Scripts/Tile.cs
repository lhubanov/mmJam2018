using System.Collections;
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

    //public string GetSpriteName()
    //{
    //    if(GetComponentInChildren<SpriteRenderer>() != null)
    //    {
    //        List<string> renderers = new List<string>();
    //        return GetComponentInChildren<SpriteRenderer>().sprite.name;
    //    }

    //    return "-1";
    //}

    //public void KillTile(string newResourceName, int resourceID = 0)
    //{
    //    isDead = true;
    //    Sprite[] resources = Resources.LoadAll<Sprite>(newResourceName);

    //    //GetComponentInChildren<SpriteRenderer>().sprite = resources[resourceID];
    //    SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
    //    for(int i = 0; i < renderers.Length; i++) {
    //        renderers[i].sprite = resources[resourceID];
    //    }
    //}

    private int GetResourceID(string spriteName)
    {
        char resourceID = spriteName[spriteName.Length - 1];
        return (char.GetNumericValue(resourceID) != -1) ? (int)char.GetNumericValue(resourceID) : 0;
    }

    public void DrainTile()
    {
        DrainTile(transform);
        isDead = true;
    }

    private void DrainTile(Transform transform)
    {
        foreach (Transform t in transform)
        {
            if (t.GetComponent<SpriteRenderer>() != null)
            {
                string spriteName;
                if (DeadTileLookup.deadSprites.TryGetValue(t.GetComponentInChildren<SpriteRenderer>().sprite.name, out spriteName))
                {
                    Sprite[] resources = Resources.LoadAll<Sprite>(spriteName);
                    t.GetComponentInChildren<SpriteRenderer>().sprite = resources[GetResourceID(spriteName)];
                }
            }

            if (t.childCount > 0) {
                DrainTile(t);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player") {
            PlayerController player = other.GetComponent<PlayerController>();
            if (Input.GetButton("Fire1") && !isDead) {

                GameObject.Find("HealthBars").GetComponent<MommaPower>().UpdatePupCurrentHealth(5);

                var playerAnimator = GameObject.Find("Player").GetComponent<Animator>();
                playerAnimator.Play("drainLife");

                GameObject.Find("WorldDrainSoundManager").GetComponent<WorldDrainSoundManager>().PlayWorldDrainSound();

                //alreadyDead = true;

                //DrainLifeFromChildren(transform);
            }
        }
    }
    
    // I hate myself
    //private void DrainLifeFromChildren(Transform transform)
    //{
    //    foreach (Transform t in transform)
    //    { 
    //        if ((t.GetComponent<SpriteRenderer>()) != null)
    //        {
    //            if (t.GetComponent<SpriteRenderer>().sprite.name == "grass pattern")
    //            {
    //                Sprite resource = Resources.Load<Sprite>("grass pattern dead");
    //                t.GetComponent<SpriteRenderer>().sprite = resource;
    //            }

    //            if (t.GetComponent<SpriteRenderer>().sprite.name == "tree regular")
    //            {
    //                Sprite resource = Resources.Load<Sprite>("tree dead");
    //                t.GetComponent<SpriteRenderer>().sprite = resource;
    //            }

    //            if (t.GetComponent<SpriteRenderer>().sprite.name == "bushes_0")
    //            {
    //                Sprite[] resource = Resources.LoadAll<Sprite>("bushes dead");
    //                t.GetComponent<SpriteRenderer>().sprite = resource[0];
    //            }

    //            if (t.GetComponent<SpriteRenderer>().sprite.name == "bushes_1")
    //            {
    //                Sprite[] resource = Resources.LoadAll<Sprite>("bushes dead");
    //                t.GetComponent<SpriteRenderer>().sprite = resource[1];
    //            }

    //            if (t.GetComponent<SpriteRenderer>().sprite.name == "bushes_2")
    //            {
    //                Sprite[] resource = Resources.LoadAll<Sprite>("bushes dead");
    //                t.GetComponent<SpriteRenderer>().sprite = resource[2];
    //            }

    //            if (t.GetComponent<SpriteRenderer>().sprite.name == "bushes_3")
    //            {
    //                Sprite[] resource = Resources.LoadAll<Sprite>("bushes dead");
    //                t.GetComponent<SpriteRenderer>().sprite = resource[3];
    //            }

    //            if (t.GetComponent<SpriteRenderer>().sprite.name == "flowers_0")
    //            {
    //                Sprite[] resource = Resources.LoadAll<Sprite>("flowers dead");
    //                t.GetComponent<SpriteRenderer>().sprite = resource[0];
    //            }

    //            if (t.GetComponent<SpriteRenderer>().sprite.name == "flowers_1")
    //            {
    //                Sprite[] resource = Resources.LoadAll<Sprite>("flowers dead");
    //                t.GetComponent<SpriteRenderer>().sprite = resource[1];
    //            }

    //            if (t.GetComponent<SpriteRenderer>().sprite.name == "flowers_2")
    //            {
    //                Sprite[] resource = Resources.LoadAll<Sprite>("flowers dead");
    //                t.GetComponent<SpriteRenderer>().sprite = resource[2];
    //            }

    //            if (t.GetComponent<SpriteRenderer>().sprite.name == "flowers_3")
    //            {
    //                Sprite[] resource = Resources.LoadAll<Sprite>("flowers dead");
    //                t.GetComponent<SpriteRenderer>().sprite = resource[3];
    //            }
    //        }

    //        if(t.childCount > 0) { 
    //            DrainLifeFromChildren(t);
    //        }
    //    }


    //}

    //private IEnumerator WaitForAnimation(Animator playerAnimator)
    //{
    //    playerAnimator.Play("drainLife");
    //    AnimatorClipInfo[] clipInfo = playerAnimator.GetCurrentAnimatorClipInfo(0);
    //    yield return new WaitForSeconds(clipInfo[0].clip.length);
    //}
}
