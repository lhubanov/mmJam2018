
namespace Assets.Scripts.States
{
    public class EndingState : StateBase
    {
        private bool isDialogueDone = false;

        public override void OnEnter(StateMachine stateMachine)
        {
            base.OnEnter(stateMachine);

            PlayDialogue(stateMachine);
            PlayMusic(stateMachine);

            // Play fade to white/black animation
        }

        public override void Update(StateMachine stateMachine)
        {
            if (isDialogueDone)
            {
                FMOD.Studio.PLAYBACK_STATE playbackState;
                stateMachine.FinalDialogueInstance.getPlaybackState(out playbackState);

                if (playbackState == FMOD.Studio.PLAYBACK_STATE.STOPPED) {
                    PlayMusic(stateMachine);
                }
            }

        }

        public override void PlayDialogue(StateMachine stateMachine)
        {
            if (!isDialogueDone)
            {
                base.SetCurrentPlayingDialogue(stateMachine, stateMachine.FinalDialogueInstance);
                stateMachine.FinalDialogueInstance.start();
                StopMusic(stateMachine);

                isDialogueDone = true;
            }
        }

        public override void PlayMusic(StateMachine stateMachine)
        {
            FMOD.Studio.PLAYBACK_STATE playbackState;
            stateMachine.EndingMusicInstance.getPlaybackState(out playbackState);

            if (playbackState != FMOD.Studio.PLAYBACK_STATE.PLAYING) {  
                base.SetCurrentPlayingMusic(stateMachine, stateMachine.EndingMusicInstance);
                stateMachine.EndingMusicInstance.start();
            }
        }

        public override void AdvanceState(StateMachine stateMachine)
        {
            stateMachine.CurrentState.OnExit(stateMachine);
            stateMachine.CurrentState = new StartMenuState();
            stateMachine.CurrentState.OnEnter(stateMachine);
        }
    }
}
