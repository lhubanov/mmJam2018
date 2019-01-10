using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.States;

namespace Assets.Scripts.States
{
    public class StartState : StateBase
    {
        private bool isDialogueDone;

        public override void OnEnter(StateMachine stateMachine)
        {
            base.OnEnter(stateMachine);

            isDialogueDone = false;
            stateMachine.MomCurrentHealth = 100;
        }

        public override void Update(StateMachine stateMachine)
        {
            // FIXME: Replace with an event
            if (isDialogueDone)
            { 
                FMOD.Studio.PLAYBACK_STATE playbackState;
                stateMachine.SpeechIntroInstance.getPlaybackState(out playbackState);

                if (playbackState == FMOD.Studio.PLAYBACK_STATE.STOPPING) {
                    stateMachine.MomStartsDying = true;
                }
            }
        }

        public override void PlayDialogue(StateMachine stateMachine)
        {
            if (!isDialogueDone)
            {
                base.SetCurrentPlayingDialogue(stateMachine, stateMachine.SpeechIntroInstance);
                stateMachine.SpeechIntroInstance.start();
                isDialogueDone = true;
            }
        }

        public override void PlayMusic(StateMachine stateMachine)
        {
        }

        public override void AdvanceState(StateMachine stateMachine)
        {
            stateMachine.CurrentState.OnExit(stateMachine);
            stateMachine.CurrentState = new FirstTimeBackState();
            stateMachine.CurrentState.OnEnter(stateMachine);
        }
    }
}
