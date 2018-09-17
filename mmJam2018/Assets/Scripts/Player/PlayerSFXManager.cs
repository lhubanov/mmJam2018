using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class PlayerSFXManager : MonoBehaviour
{
    [EventRef]
    public string DrainSound;
    private EventInstance drainSoundInstance;
 
    void Start ()
    {
        drainSoundInstance = RuntimeManager.CreateInstance(DrainSound);
    }
	
	void Update ()
    {
        StopLifeDrainSound();
    }

    public void PlayLifeDrainSound()
    {
        FMOD.Studio.PLAYBACK_STATE state;
        drainSoundInstance.getPlaybackState(out state);
        if (state != PLAYBACK_STATE.PLAYING)
        {
            drainSoundInstance.start();
        }
    }

    private void StopLifeDrainSound()
    {
        FMOD.Studio.PLAYBACK_STATE state;
        drainSoundInstance.getPlaybackState(out state);
        if (state == PLAYBACK_STATE.STOPPING)
        {
            drainSoundInstance.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }
}
