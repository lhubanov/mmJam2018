using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchInteraction3 : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && Input.GetButton("Fire1"))
        {
            //Play animation!
            GetComponentInParent<Animator>().Play("BranchDisappear3");
            GameObject.Find("Mom").GetComponent<DialogueAudioHandler>().Stage3Complete = true;
        }
    }
}
