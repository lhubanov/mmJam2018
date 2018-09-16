using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Scripts.States
{
    public abstract class StateBase : IState
    {
        public virtual void OnEnter(StateMachine stateMachine) { }
        public virtual void OnExit(StateMachine stateMachine) { }
        public virtual void ToState(StateMachine stateMachine, IState nextState)
        {
            stateMachine.CurrentState.OnExit(stateMachine);
            stateMachine.CurrentState = nextState;
            stateMachine.CurrentState.OnEnter(stateMachine);
        }

        public abstract void Update(StateMachine stateMachine);

        public abstract void PlayMusic(StateMachine stateMachine);
        public abstract void PlayDialogue(StateMachine stateMachine);

        // This was in the talk, but not sure if necessary here
        //public abstract void HandleInput(StateMachine stateMachine, Input Input);
    }
}
