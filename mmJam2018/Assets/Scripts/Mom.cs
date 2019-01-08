using System.Collections;
using UnityEngine;
using FMOD.Studio;

public class Mom : MonoBehaviour
{
    public StateMachine World;

    public float RechargeSpeed = 5;
    public float DrainAmount = 3;
    public float DrainSpeed = 2;

    private void Update()
    {
        if (World.MomStartsDying) {
            StartCoroutine(LoseHealthIdly());
            World.MomStartsDying = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null) {
            World.CurrentState.PlayDialogue(World);
        }
    }

    // Transcends whatever world state.. I think
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null && Input.GetButton("Fire2"))
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
