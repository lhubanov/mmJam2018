using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEnergyController : MonoBehaviour
{
    //maybe add a var keeping track of land tile energy
    private Color startingTileColor;

    void Start()
    {
        startingTileColor = GetComponent<Renderer>().material.color;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player") {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player.ActionButtonPressed) {
                GetComponentInParent<Renderer>().material.color = new Color(startingTileColor.r - 0.05f, startingTileColor.g - 0.05f, startingTileColor.b - 0.05f);
                startingTileColor = 
            }
        }
    }
}
