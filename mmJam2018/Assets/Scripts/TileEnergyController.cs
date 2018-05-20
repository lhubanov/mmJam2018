using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEnergyController : MonoBehaviour
{
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


                //play animation
                var playerAnimator = GameObject.Find("Player").GetComponent<Animator>();
                playerAnimator.Play("drainLife");
                //WaitForAnimation(playerAnimator);
                //when animation done

                alreadyDead = true;
                //GameObject.Find("Player").energy


                foreach (Transform t in transform)
                {
                    if((t.GetComponent<SpriteRenderer>()) != null) { 
                        if (t.GetComponent<SpriteRenderer>().sprite.name == "grass pattern"){ 
                            Sprite resource = Resources.Load<Sprite>("grass_pattern_dead");
                            t.GetComponent<SpriteRenderer>().sprite = resource;
                        }

                        //add tree check w/ live and dead
                        //add bush check w/ live and dead
                        
                    }
                }
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
