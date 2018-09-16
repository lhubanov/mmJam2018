using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

// The mom, effectively
public class DialogueAudioHandler : MonoBehaviour
{

    public bool Stage1Complete = true;
    public bool Stage1DialogueDone = false;
    public bool Stage2Complete = false;
    public bool Stage2DialogueDone = false;
    public bool Stage3Complete = false;
    public bool Stage3DialogueDone = false;

    //public  AudioClip momLine1;
    //public  AudioClip childLine1;

    //private AudioSource audio;


    [EventRef]
    public string RechargeSound;
    private EventInstance rechargeInstance;

    [EventRef]
    public string GameplayMusic;
    private EventInstance gameplayMusicInstance;

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
        gameplayMusicInstance = RuntimeManager.CreateInstance(GameplayMusic);
        speechIntroInstance = RuntimeManager.CreateInstance(SpeechIntro);
        speech1Instance = RuntimeManager.CreateInstance(Speech1);
        speech2Instance = RuntimeManager.CreateInstance(Speech2);

        //player = GameObject.Find("Player").GetComponent<PlayerController>();

        gameplayMusicInstance.start();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player") {
            PlayDialogue();
        }
    }

    private void OnTriggerStay(Collider other)
    { 
        FMOD.Studio.PLAYBACK_STATE rechargeSoundState;

        if (other.tag == "Player" && Input.GetButton("Fire2"))
        {
            rechargeInstance.getPlaybackState(out rechargeSoundState);
            if (rechargeSoundState != PLAYBACK_STATE.PLAYING) {
                rechargeInstance.start();
            }

            GameObject.Find("HealthBars").GetComponent<MommaPower>().UpdateMommaCurrentHealth(5);
        }
    }

    private void PlayDialogue()
    {
        if (Stage1Complete && !Stage1DialogueDone) {
            Stage1DialogueDone = true;
            //StartCoroutine(PlayDialogue1());
            RuntimeManager.PlayOneShot(SpeechIntro);

        } if (Stage2Complete && !Stage2DialogueDone) {
            Stage2DialogueDone = true;
            //StartCoroutine(PlayDialogue1());
            RuntimeManager.PlayOneShot(Speech1);

        } if (Stage3Complete && !Stage3DialogueDone) {
            Stage3DialogueDone = true;
            RuntimeManager.PlayOneShot(Speech2);

            //StartCoroutine(PlayDialogue1());
        }
    }
}
