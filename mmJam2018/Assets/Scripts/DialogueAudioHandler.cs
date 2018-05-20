using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAudioHandler : MonoBehaviour {

    public  AudioClip momLine1;
    public  AudioClip childLine1;

    private AudioSource audio;
    private PlayerController player;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.loop = false;
        //StartCoroutine(playDialogue());
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            player.Talking = true;
            //&& at whatever point of the story
            StartCoroutine(playDialogue());
        }
    }

    IEnumerator playDialogue()
    {
        var animator = GetComponentInChildren<Animator>();
        animator.Play("momBeingSuspicious");
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

        animator.Play("momIdle");
        player.Talking = false;
    }
}
