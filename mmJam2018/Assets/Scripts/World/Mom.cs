using System.Collections;
using UnityEngine;
using Assets.Scripts;

// Mom should be of IDie, but this causes a case where the player can kill Mom :D
public class Mom : MonoBehaviour //, IDie
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

    private Coroutine healthLossRoutine;
    private bool notHealedYet;

    private void Start()
    {
        healthLossRoutine = null;
        notHealedYet = true;
    }

    private void Update()
    {
        World.CurrentState.Update(World);

        if (World.MomStartsDying) {
            healthLossRoutine = StartCoroutine(LoseHealthIdly());
            World.MomStartsDying = false;
        }

        if (IsHealthBelowThreshold()) {
            World.CurrentState.PlayLowHealthSound(World);
        }

        if (HasNoHealth()) {
            Die();
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

                if(notHealedYet)
                {
                    notHealedYet = false;
                    World.CurrentState.AdvanceState(World);
                    StopCoroutine(healthLossRoutine);

                    DrainSpeed += DrainSpeed / 2;
                    healthLossRoutine = StartCoroutine(LoseHealthIdly());
                    World.CurrentState.PlayDialogue(World);
                }
            }
        }
    }

    public void Die()
    {
        World.CurrentState.PlayEnding(World);
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

    private bool HasNoHealth()
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