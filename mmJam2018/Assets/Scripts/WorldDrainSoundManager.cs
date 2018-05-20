using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;


public class WorldDrainSoundManager : MonoBehaviour
{
    [EventRef]
    public string CloakSound;
    private EventInstance walkingInstance;

    private void Start()
    {
        walkingInstance = RuntimeManager.CreateInstance(CloakSound);
    }

    public void PlayWorldDrainSound()
    {
        FMOD.Studio.PLAYBACK_STATE walkingSoundState;
        walkingInstance.getPlaybackState(out walkingSoundState);
        if (walkingSoundState != PLAYBACK_STATE.PLAYING) {
            walkingInstance.start();
        }
    }

    public void Update()
    {
        FMOD.Studio.PLAYBACK_STATE walkingSoundState;
        walkingInstance.getPlaybackState(out walkingSoundState);
        if (walkingSoundState == PLAYBACK_STATE.STOPPING) {
            walkingInstance.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }
}
