using System.Collections;
using UnityEngine;
using Assets.Scripts.States;

//FIXME: Mom should be of IDie, but this causes a case where the player can kill Mom :D
public class Mom : MonoBehaviour //, IDie
{
    [SerializeField]
    private StateMachine World;

    [SerializeField]
    private MomHealthManager healthManager;

    private void Start()
    {
        healthManager = GetComponent<MomHealthManager>();
    }

    private void Update()
    {
        World.CurrentState.Update(World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null) {
            World.CurrentState.PlayDialogue(World);
        }
    }
}