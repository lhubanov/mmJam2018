using UnityEngine;
using Assets.Scripts.States;

using FMOD.Studio;

public class Mom : MonoBehaviour
{
    public StateMachine World;

	void Start ()
    {
        World.CurrentState = new StartState();
        World.CurrentState.OnEnter(World);
	}
	
	void Update ()
    {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            World.CurrentState.PlayDialogue(World);
        }
    }

    // Transcends whatever world state.. I think
    private void OnTriggerStay(Collider other)
    {
        FMOD.Studio.PLAYBACK_STATE rechargeSoundState;

        if (other.tag == "Player" && Input.GetButton("Fire2"))
        {
            World.RechargeInstance.getPlaybackState(out rechargeSoundState);
            if (rechargeSoundState != PLAYBACK_STATE.PLAYING)
            {
                World.RechargeInstance.start();
            }

            GameObject.Find("HealthBars").GetComponent<MommaPower>().UpdateMommaCurrentHealth(5);
        }
    }
}
