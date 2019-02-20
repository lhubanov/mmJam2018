using UnityEngine;
using Assets.Scripts.States;

public class WhiteScreenFadeControl : MonoBehaviour
{
    [SerializeField]
    private StateMachine stateMachine;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetButton("Teleport") && !(stateMachine.CurrentState is StartMenuState)) {
            animator.Play("FadeToWhite");
        }
    }
}