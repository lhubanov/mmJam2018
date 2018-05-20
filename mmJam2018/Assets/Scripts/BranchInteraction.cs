using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchInteraction : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player" && Input.GetButton("Fire1"))
        {
            GetComponentInParent<Animator>().Play("BranchDisappear1");
            GameObject.Find("Mom").GetComponent<DialogueAudioHandler>().Stage1Complete = true;
        }
    }
}
