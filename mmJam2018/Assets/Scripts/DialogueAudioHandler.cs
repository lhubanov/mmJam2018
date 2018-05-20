using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class DialogueAudioHandler : MonoBehaviour {

    public bool Stage1Complete = false;
    public bool Stage1DialogueDone = false;
    public bool Stage2Complete = false;
    public bool Stage2DialogueDone = false;
    public bool Stage3Complete = false;
    public bool Stage3DialogueDone = false;

    //public  AudioClip momLine1;
    //public  AudioClip childLine1;

    //private AudioSource audio;
    private PlayerController player;


    [EventRef]
    public string RechargeSound;
    private EventInstance rechargeInstance;

    [EventRef]
    public string SnoringSound;
    private EventInstance snoringInstance;

    [EventRef]
    public string SpeechIntro;
    private EventInstance speechIntroInstance;

    [EventRef]
    public string Speech1;
    private EventInstance speech1Instance;

    [EventRef]
    public string Speech2;
    private EventInstance speech2Instance;

    void Start()
    {
        rechargeInstance = RuntimeManager.CreateInstance(RechargeSound);
        snoringInstance = RuntimeManager.CreateInstance(SnoringSound);
        speechIntroInstance = RuntimeManager.CreateInstance(SpeechIntro);
        speech1Instance = RuntimeManager.CreateInstance(Speech1);
        speech2Instance = RuntimeManager.CreateInstance(Speech2);

        //audio = GetComponent<AudioSource>();
        //audio.loop = false;

        snoringInstance.start();

        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player") {
            PlayDialogue();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        FMOD.Studio.PLAYBACK_STATE snoringSoundState;
        FMOD.Studio.PLAYBACK_STATE rechargeSoundState;

        if (other.tag == "Player" && Input.GetButton("Fire2"))
        {
            snoringInstance.getPlaybackState(out snoringSoundState);
            if (snoringSoundState != PLAYBACK_STATE.PLAYING) {
                snoringInstance.stop(STOP_MODE.ALLOWFADEOUT);
            }

            rechargeInstance.getPlaybackState(out rechargeSoundState);
            if (rechargeSoundState != PLAYBACK_STATE.PLAYING) {
                rechargeInstance.start();
            }

            GameObject.Find("HealthBars").GetComponent<MommaPower>().UpdateMommaCurrentHealth(5);
        }

        rechargeInstance.getPlaybackState(out rechargeSoundState);
        if (rechargeSoundState != PLAYBACK_STATE.PLAYING) {
            snoringInstance.start();
        }
    }

    private void PlayDialogue()
    {
        if (Stage1Complete && !Stage1DialogueDone) {
            Stage1DialogueDone = true;
            //StartCoroutine(PlayDialogue1());


        } if (Stage2Complete && !Stage2DialogueDone) {
            Stage2DialogueDone = true;
            //StartCoroutine(PlayDialogue1());
        } if (Stage3Complete && !Stage3DialogueDone) {
            Stage3DialogueDone = true;
            //StartCoroutine(PlayDialogue1());
        }
    }

    //IEnumerator PlayDialogue1()
    //{
    //    player.Talking = true;

    //    var animator = GetComponentInChildren<Animator>();
    //    animator.Play("momBeingSuspicious");
    //    audio.clip = momLine1;
    //    audio.Play();
    //    yield return new WaitForSeconds(audio.clip.length);
    //    audio.clip = childLine1;
    //    audio.Play();
    //    yield return new WaitForSeconds(audio.clip.length);
    //    audio.clip = momLine1;
    //    audio.Play();
    //    yield return new WaitForSeconds(audio.clip.length);
    //    audio.clip = childLine1;
    //    audio.Play();

    //    animator.Play("momIdle");
    //    player.Talking = false;
    //}
}
