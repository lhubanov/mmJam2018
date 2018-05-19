using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAudioHandler : MonoBehaviour {

    public  AudioClip momLine1;
    public  AudioClip childLine1;
    private AudioSource audio;
    void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.loop = false;
        //StartCoroutine(playDialogue());
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //&& at whatever point of the story
            StartCoroutine(playDialogue());
        }
    }

    IEnumerator playDialogue()
    {
        audio.clip = momLine1;
        audio.Play();
        yield return new WaitForSeconds(audio.clip.length);
        audio.clip = childLine1;
        audio.Play();
        yield return new WaitForSeconds(audio.clip.length);
        audio.clip = momLine1;
        audio.Play();
        yield return new WaitForSeconds(audio.clip.length);
        audio.clip = childLine1;
        audio.Play();
    }
}
