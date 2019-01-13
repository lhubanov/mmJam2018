
namespace Assets.Scripts.States
{
    public class StartMenuState : StateBase
    {
        public override void OnEnter(StateMachine stateMachine)
        {
            base.OnEnter(stateMachine);

            stateMachine.Initialize();

            base.StopMusic(stateMachine);
            PlayMusic(stateMachine);

            stateMachine.MomCurrentHealth = stateMachine.MomMaxHealth;
        }

        public override void Update(StateMachine stateMachine)
        {
        }

        public override void PlayDialogue(StateMachine stateMachine)
        {
        }

        public override void PlayMusic(StateMachine stateMachine)
        {
            FMOD.Studio.PLAYBACK_STATE playbackState;
            stateMachine.MenuMusicInstance.getPlaybackState(out playbackState);

            if (playbackState != FMOD.Studio.PLAYBACK_STATE.STARTING &&
                playbackState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
            { 
                base.SetCurrentPlayingMusic(stateMachine, stateMachine.MenuMusicInstance);
                stateMachine.MenuMusicInstance.start();
            }
        }

        public override void AdvanceState(StateMachine stateMachine)
        {
            stateMachine.CurrentState.OnExit(stateMachine);
            stateMachine.CurrentState = new StartState();
            stateMachine.CurrentState.OnEnter(stateMachine);
        }
    }
}
