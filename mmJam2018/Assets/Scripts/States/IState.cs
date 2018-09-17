using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Scripts.States
{
    public interface IState
    {
        void OnEnter(StateMachine stateMachine);
        void OnExit(StateMachine stateMachine);
        void Update(StateMachine stateMachine);

        void ToState(StateMachine stateMachine, IState nextState);
        void AdvanceState(StateMachine stateMachine);

        void PlayMusic(StateMachine stateMachine);
        void PlayDialogue(StateMachine stateMachine);
    }
}
