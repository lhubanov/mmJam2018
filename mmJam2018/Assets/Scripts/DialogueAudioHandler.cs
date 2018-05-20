using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAudioHandler : MonoBehaviour {

    public bool Stage1Complete = false;
    public bool Stage1DialogueDone = false;
    public bool Stage2Complete = false;
    public bool Stage2DialogueDone = false;
    public bool Stage3Complete = false;
    public bool Stage3DialogueDone = false;

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
            //&& at whatever point of the story
            //StartCoroutine(playDialogue());
            PlayDialogue();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player" && Input.GetButton("Fire2"))
        {
            GameObject.Find("HealthBars").GetComponent<MommaPower>().UpdateMommaCurrentHealth(5);
        }
    }

    private void PlayDialogue()
    {
        if (Stage1Complete && !Stage1DialogueDone) {
            Stage1DialogueDone = true;
            StartCoroutine(PlayDialogue1());
        } if (Stage2Complete && !Stage2DialogueDone) {
            Stage2DialogueDone = true;
            StartCoroutine(PlayDialogue1());
        } if (Stage3Complete && !Stage3DialogueDone) {
            Stage3DialogueDone = true;
            StartCoroutine(PlayDialogue1());
        }
    }

    IEnumerator PlayDialogue1()
    {
        player.Talking = true;

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
