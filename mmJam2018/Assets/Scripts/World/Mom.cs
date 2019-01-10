using System.Collections;
using UnityEngine;
using FMOD.Studio;

public class Mom : MonoBehaviour
{
    [SerializeField]
    private StateMachine World;

    [SerializeField]
    private float RechargeSpeed = 5;

    [SerializeField]
    private float DrainAmount = 3;

    [SerializeField]
    private float DrainSpeed = 2;

    [SerializeField]
    private float LowHealthThreshold = 20;



    private void Update()
    {
        World.CurrentState.Update(World);

        if (World.MomStartsDying) {
            StartCoroutine(LoseHealthIdly());
            World.MomStartsDying = false;
        }

        if (IsHealthBelowThreshold()) {
            World.CurrentState.PlayLowHealthSound(World);
        }

        if (IsDead()) {
            World.CurrentState.PlayEnding(World);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null) {
            World.CurrentState.PlayDialogue(World);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null && Input.GetButton("DeliverEnergy"))
        {
            if( World.MomCurrentHealth < World.MomMaxHealth && 
                World.HeldEnergy > World.MomMinHealth)
            { 
                World.CurrentState.PlayRechargeSound(World);
                IncreaseHealth(RechargeSpeed);
            }
        }
    }

    private void IncreaseHealth(float amount)
    {
        World.MomCurrentHealth += amount;
        World.HeldEnergy -= amount;
    }


    private bool IsHealthBelowThreshold()
    {
        return (World.MomCurrentHealth < (World.MomMinHealth + LowHealthThreshold));
    }

    private bool IsDead()
    {
        return (World.MomCurrentHealth <= World.MomMinHealth);
    }

    private IEnumerator LoseHealthIdly()
    {
        while (World.MomCurrentHealth > 0) {
            World.MomCurrentHealth -= DrainAmount;
            yield return new WaitForSeconds(DrainSpeed);
        }
    }

}