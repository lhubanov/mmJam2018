using System.Collections;
using UnityEngine;
using Assets.Scripts.States;

public class MomHealthManager : MonoBehaviour
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
    private Coroutine fadeRoutine;
    private bool notHealedYet;

    private void Start()
    {
        healthLossRoutine = null;
        notHealedYet = true;
    }

    private void OnEnable()
    {
        World.OnMomStartsDying += StartDeathCoroutines;
    }

    private void OnDisable()
    {
        World.OnMomStartsDying -= StartDeathCoroutines;
    }

    private void Update()
    {
        if (!(World.CurrentState is EndingState))
        {
            if (IsHealthBelowThreshold()) {
                World.CurrentState.PlayLowHealthSound(World);
            }

            if (HasNoHealth()) {
                Die();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null && Input.GetButton("DeliverEnergy"))
        {
            if (World.MomCurrentHealth < World.MomMaxHealth && World.HeldEnergy > World.MomMinHealth)
            {
                World.CurrentState.PlayRechargeSound(World);
                IncreaseHealth(RechargeSpeed);

                World.FadeAmount = 0;

                if (notHealedYet)
                {
                    notHealedYet = false;
                    World.CurrentState.AdvanceState(World);
                    World.CurrentState.PlayDialogue(World);
                }
            }
        }
    }

    private void IncreaseHealth(float amount)
    {
        World.MomCurrentHealth += amount;
        World.HeldEnergy -= amount;
    }

    private bool IsHealthBelowThreshold() { return (World.MomCurrentHealth == (World.MomMinHealth + LowHealthThreshold) || World.MomCurrentHealth == (World.MomMinHealth + (LowHealthThreshold / 2))); }
    private bool HasNoHealth() { return (World.MomCurrentHealth <= World.MomMinHealth); }

    public void Die()
    {
        World.CurrentState.PlayEnding(World);
    }

    public void StartDeathCoroutines()
    {
        healthLossRoutine = StartCoroutine(LoseHealthIdly());
        fadeRoutine = StartCoroutine(FadeToBlackSlowly());
    }

    private IEnumerator LoseHealthIdly()
    {
        while (World.MomCurrentHealth > World.MomMinHealth)
        {
            World.MomCurrentHealth -= DrainAmount;
            yield return new WaitForSeconds(DrainSpeed);
        }
    }

    private IEnumerator FadeToBlackSlowly()
    {
        while (World.MomCurrentHealth > World.MomMinHealth)
        {
            if (World.MomCurrentHealth < (World.MomMinHealth + LowHealthThreshold))
            {
                World.FadeAmount += (255 / LowHealthThreshold) * 2;
            }
            yield return new WaitForSeconds(DrainSpeed);
        }
    }
}

