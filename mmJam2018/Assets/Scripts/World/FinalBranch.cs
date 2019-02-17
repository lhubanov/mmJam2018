using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.World
{
    public class FinalBranch : Branch
    {
        public override void Die()
        {
            base.Die();

            stateMachine.FadeAmount = 255;
            stateMachine.CurrentState.AdvanceState(stateMachine);
        }
    }
}
