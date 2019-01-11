
namespace Assets.Scripts.States
{
    public class StartMenuState : StateBase
    {
        public override void OnEnter(StateMachine stateMachine)
        {
            base.OnEnter(stateMachine);

            stateMachine.Initialize();
            PlayMusic(stateMachine);

            stateMachine.MomCurrentHealth = 100;
        }

        public override void Update(StateMachine stateMachine)
        {
        }

        public override void PlayDialogue(StateMachine stateMachine)
        {
        }

        public override void PlayMusic(StateMachine stateMachine)
        {
            base.SetCurrentPlayingMusic(stateMachine, stateMachine.MenuMusicInstance);
            stateMachine.MenuMusicInstance.start();
        }

        public override void AdvanceState(StateMachine stateMachine)
        {
            stateMachine.CurrentState.OnExit(stateMachine);
            stateMachine.CurrentState = new StartState();
            stateMachine.CurrentState.OnEnter(stateMachine);
        }
    }
}
