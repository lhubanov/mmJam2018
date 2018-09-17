using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.States
{
    public class OneBranchDeadState : StateBase
    {
        private bool isDialogueDone = false;

        public override void OnEnter(StateMachine stateMachine)
        {
            base.OnEnter(stateMachine);
        }

        public override void Update(StateMachine stateMachine)
        {
        }

        public override void PlayDialogue(StateMachine stateMachine)
        {
            if (!isDialogueDone)
            {
                // FIXME: depending on gameplay there might be more states, actually
                stateMachine.Speech1Instance.start();
                isDialogueDone = true;
            }
        }

        public override void PlayMusic(StateMachine stateMachine)
        {
            // FIXME: Is there other music?
            //stateMachine.GameplayMusicInstance.start();
        }

        public override void AdvanceState(StateMachine stateMachine)
        {
            stateMachine.CurrentState.OnExit(stateMachine);
            stateMachine.CurrentState = new TwoBranchesDeadState();
            stateMachine.CurrentState.OnEnter(stateMachine);
        }
    }
}
