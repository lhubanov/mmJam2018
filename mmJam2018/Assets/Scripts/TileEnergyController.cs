using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class TileEnergyController : MonoBehaviour
{
    [EventRef]
    public string DrainSound;
    private EventInstance drainInstance;

    //maybe add a var keeping track of land tile energy
    private Color startingTileColor;
    public bool alreadyDead = false;

    void Start()
    {
        startingTileColor = GetComponentInChildren<SpriteRenderer>().color;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player") {
            PlayerController player = other.GetComponent<PlayerController>();
            if (Input.GetButton("Fire1") && !alreadyDead) {

                GameObject.Find("HealthBars").GetComponent<MommaPower>().UpdatePupCurrentHealth(5);
                //play animation
                var playerAnimator = GameObject.Find("Player").GetComponent<Animator>();
                playerAnimator.Play("drainLife");
                //WaitForAnimation(playerAnimator);
                //when animation done

                alreadyDead = true;
                //GameObject.Find("Player").energy

                DrainLifeFromChildren(transform);
            }
        }
    }
    
    // I hate myself
    private void DrainLifeFromChildren(Transform transform)
    {
        foreach (Transform t in transform)
        { 
            if ((t.GetComponent<SpriteRenderer>()) != null)
            {
                if (t.GetComponent<SpriteRenderer>().sprite.name == "grass pattern")
                {
                    Sprite resource = Resources.Load<Sprite>("grass pattern dead");
                    t.GetComponent<SpriteRenderer>().sprite = resource;
                }

                if (t.GetComponent<SpriteRenderer>().sprite.name == "tree regular")
                {
                    Sprite resource = Resources.Load<Sprite>("tree dead");
                    t.GetComponent<SpriteRenderer>().sprite = resource;
                }

                if (t.GetComponent<SpriteRenderer>().sprite.name == "bushes_0")
                {
                    Sprite[] resource = Resources.LoadAll<Sprite>("bushes dead");
                    t.GetComponent<SpriteRenderer>().sprite = resource[0];
                }

                if (t.GetComponent<SpriteRenderer>().sprite.name == "bushes_1")
                {
                    Sprite[] resource = Resources.LoadAll<Sprite>("bushes dead");
                    t.GetComponent<SpriteRenderer>().sprite = resource[1];
                }

                if (t.GetComponent<SpriteRenderer>().sprite.name == "bushes_2")
                {
                    Sprite[] resource = Resources.LoadAll<Sprite>("bushes dead");
                    t.GetComponent<SpriteRenderer>().sprite = resource[2];
                }

                if (t.GetComponent<SpriteRenderer>().sprite.name == "bushes_3")
                {
                    Sprite[] resource = Resources.LoadAll<Sprite>("bushes dead");
                    t.GetComponent<SpriteRenderer>().sprite = resource[3];
                }

                if (t.GetComponent<SpriteRenderer>().sprite.name == "flowers_0")
                {
                    Sprite[] resource = Resources.LoadAll<Sprite>("flowers dead");
                    t.GetComponent<SpriteRenderer>().sprite = resource[0];
                }

                if (t.GetComponent<SpriteRenderer>().sprite.name == "flowers_1")
                {
                    Sprite[] resource = Resources.LoadAll<Sprite>("flowers dead");
                    t.GetComponent<SpriteRenderer>().sprite = resource[1];
                }

                if (t.GetComponent<SpriteRenderer>().sprite.name == "flowers_2")
                {
                    Sprite[] resource = Resources.LoadAll<Sprite>("flowers dead");
                    t.GetComponent<SpriteRenderer>().sprite = resource[2];
                }

                if (t.GetComponent<SpriteRenderer>().sprite.name == "flowers_3")
                {
                    Sprite[] resource = Resources.LoadAll<Sprite>("flowers dead");
                    t.GetComponent<SpriteRenderer>().sprite = resource[3];
                }
            }

            if(t.childCount > 0) { 
                DrainLifeFromChildren(t);
            }
        }


    }

    //private IEnumerator WaitForAnimation(Animator playerAnimator)
    //{
    //    playerAnimator.Play("drainLife");
    //    AnimatorClipInfo[] clipInfo = playerAnimator.GetCurrentAnimatorClipInfo(0);
    //    yield return new WaitForSeconds(clipInfo[0].clip.length);
    //}
}
