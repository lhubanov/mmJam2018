using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.States;

public class StartState : StateBase
{
    private bool isDialogueDone = false;

    public override void OnEnter(StateMachine stateMachine)
    {
        base.OnEnter(stateMachine);
        stateMachine.Initialize();
        PlayMusic(stateMachine);
    }

    public override void Update(StateMachine stateMachine)
    {
        
    }

    public override void PlayDialogue(StateMachine stateMachine)
    {
        if (!isDialogueDone) { 
            stateMachine.SpeechIntroInstance.start();
            isDialogueDone = true;
        }
    }

    public override void PlayMusic(StateMachine stateMachine)
    {
        stateMachine.GameplayMusicInstance.start();
    }
}
