using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class BranchInteraction : MonoBehaviour
{
    [EventRef]
    public string DestructionSound;

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player" && Input.GetButton("Fire1"))
        {
            GetComponentInParent<Animator>().Play("BranchDisappear1");
            RuntimeManager.PlayOneShot(DestructionSound);
            GameObject.Find("Mom").GetComponent<DialogueAudioHandler>().Stage2Complete = true;
        }
    }
}
