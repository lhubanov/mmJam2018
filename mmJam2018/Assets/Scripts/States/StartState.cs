using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.States;

public class StartState : StateBase
{
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
        stateMachine.SpeechIntroInstance.start();
    }

    public override void PlayMusic(StateMachine stateMachine)
    {
        stateMachine.GameplayMusicInstance.start();
    }
}
