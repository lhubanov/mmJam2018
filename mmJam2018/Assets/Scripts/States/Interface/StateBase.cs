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
        public virtual void PlayMusic(StateMachine stateMachine) { }
        public virtual void PlayDialogue(StateMachine stateMachine) { }

        public abstract void Update(StateMachine stateMachine);
        public abstract void AdvanceState(StateMachine stateMachine);
    }
}
