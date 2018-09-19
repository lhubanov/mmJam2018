using System.Collections;
using UnityEngine;
using FMOD.Studio;

using Assets.Scripts.States;

public class Mom : MonoBehaviour
{
    public StateMachine World;

    public float RechargeSpeed = 5;
    public float DrainAmount = 3;
    public float DrainSpeed = 2;

	void Start ()
    {
        World.CurrentState = new StartState();
        World.CurrentState.OnEnter(World);
        World.MomHealth = 100;

        // TODO FROM BEFORE: Disable UI until passing through a collider- exit of cave or sth like that
        // FIXME: This should be triggered after initial monologue is done (when UI is enabled after passing that collider)
        StartCoroutine(LoseHealthIdly());
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
        if (other.tag == "Player" && Input.GetButton("Fire2"))
        {
            if(World.MomHealth < 120 && World.HeldEnergy > 0) { 
                PlayRechargeSound();
                IncreaseHealth(RechargeSpeed);
            }
        }
    }

    private void IncreaseHealth(float amount)
    {
        World.MomHealth += amount;
        World.HeldEnergy -= amount;
    }

    private IEnumerator LoseHealthIdly()
    {
        while (World.MomHealth > 0) {
            World.MomHealth -= DrainAmount;
            yield return new WaitForSeconds(DrainSpeed);
        }
    }

    private void PlayRechargeSound()
    {
        FMOD.Studio.PLAYBACK_STATE rechargeSoundState;

        World.RechargeInstance.getPlaybackState(out rechargeSoundState);
        if (rechargeSoundState != PLAYBACK_STATE.PLAYING) {
            World.RechargeInstance.start();
        }
    }
}
