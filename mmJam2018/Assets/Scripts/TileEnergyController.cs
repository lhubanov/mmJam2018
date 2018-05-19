using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEnergyController : MonoBehaviour
{
    //maybe add a var keeping track of land tile energy
    private Color startingTileColor;
    private bool alreadyDead = false;

    void Start()
    {
        startingTileColor = GetComponent<Renderer>().material.color;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player") {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player.ActionButtonPressed && !alreadyDead) {
                //play animation
                //when animation done

                alreadyDead = true;
                GetComponentInParent<Renderer>().material.color = new Color32(60, 60, 60, 220);
            }
        }
    }
}
