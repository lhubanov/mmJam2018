
namespace Assets.Scripts.States
{
    public class GameplayState : StateBase
    {
        private bool isDialogueDone;

        public override void OnEnter(StateMachine stateMachine)
        {
            base.OnEnter(stateMachine);

            isDialogueDone = false;
        }

        public override void Update(StateMachine stateMachine)
        {
            if (isDialogueDone)
            {
                FMOD.Studio.PLAYBACK_STATE playbackState;
                stateMachine.Speech1Instance.getPlaybackState(out playbackState);

                if (playbackState == FMOD.Studio.PLAYBACK_STATE.STOPPING ||
                    playbackState == FMOD.Studio.PLAYBACK_STATE.STOPPED) {

                    //FIXME: This plays twice and messes up with the FMOD playback - use different flag or sth
                    //          And same with the end music
                    PlayMusic(stateMachine);
                }
            }
        }

        public override void PlayDialogue(StateMachine stateMachine)
        {
            if (!isDialogueDone)
            {
                base.StopMusic(stateMachine);
                base.SetCurrentPlayingDialogue(stateMachine, stateMachine.Speech1Instance);
                stateMachine.Speech1Instance.start();

                isDialogueDone = true;
            }
        }

        public override void PlayMusic(StateMachine stateMachine)
        {
            base.SetCurrentPlayingMusic(stateMachine, stateMachine.GameplayMusicInstance);
            stateMachine.GameplayMusicInstance.start();
        }

        public override void AdvanceState(StateMachine stateMachine)
        {
            stateMachine.CurrentState.OnExit(stateMachine);
            stateMachine.CurrentState = new EndingState();
            stateMachine.CurrentState.OnEnter(stateMachine);
        }
    }
}
