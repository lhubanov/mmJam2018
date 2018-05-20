using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchInteraction2 : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && Input.GetButton("Fire1"))
        {
            GetComponentInParent<Animator>().Play("BranchDisappear2");
            GameObject.Find("Mom").GetComponent<DialogueAudioHandler>().Stage2Complete = true;
        }
    }
}
