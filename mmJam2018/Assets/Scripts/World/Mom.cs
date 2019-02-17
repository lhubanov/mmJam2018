using UnityEngine;
using Assets.Scripts;

public class Mom : MonoBehaviour, IDie
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

    public void Die()
    {
        healthManager.Die();
    }
}