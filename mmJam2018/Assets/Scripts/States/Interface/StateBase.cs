using FMOD.Studio;

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
        public abstract void AdvanceState(StateMachine stateMachine);

        public virtual void PlayMusic(StateMachine stateMachine) { }
        public virtual void StopMusic(StateMachine stateMachine)
        {
            stateMachine.CurrentMusic.stop(STOP_MODE.ALLOWFADEOUT);
        }

        public virtual void PlayDialogue(StateMachine stateMachine) { }
        public virtual void StopDialogue(StateMachine stateMachine)
        {
            stateMachine.CurrentDialogue.stop(STOP_MODE.ALLOWFADEOUT);
        }

        public virtual void SetCurrentPlayingMusic(StateMachine stateMachine, EventInstance musicInstance)
        {
            stateMachine.CurrentMusic = musicInstance;
        }

        public virtual void SetCurrentPlayingDialogue(StateMachine stateMachine, EventInstance musicInstance)
        {
            stateMachine.CurrentDialogue = musicInstance;
        }

        public virtual void PlayLowHealthSound(StateMachine stateMachine)
        {
            stateMachine.LowHealthInstance.start();
        }

        public virtual void PlayRechargeSound(StateMachine stateMachine)
        {
            FMOD.Studio.PLAYBACK_STATE rechargeSoundState;

            stateMachine.RechargeInstance.getPlaybackState(out rechargeSoundState);
            if (rechargeSoundState != PLAYBACK_STATE.PLAYING)
            {
                stateMachine.RechargeInstance.start();
            }
        }

        public virtual void PlayEnding(StateMachine stateMachine)
        {
            stateMachine.FinalDialogueInstance.start();
            // Play fade to white/black animation
        }
    }
}
