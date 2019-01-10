using UnityEngine;
using Assets.Scripts.States;

using FMODUnity;
using FMOD.Studio;

[CreateAssetMenu(menuName="World State Machine")]
public class StateMachine : ScriptableObject
{
    public IState CurrentState;

    // HUD Bars references
    public float    HeldEnergy;
    public float    MomCurrentHealth;

    public float    MomMaxHealth;
    public float    MomMinHealth;

    public float    PlayerMovementSlowdown;
    public bool     MomStartsDying; // FIXME: This is when you start using events

    // Audio references
    [EventRef]
    public string SnoringSound;
    public EventInstance SnoringInstance;

    [EventRef]
    public string RechargeSound;
    public EventInstance RechargeInstance;

    [EventRef]
    public string LowHealthSound;
    public EventInstance LowHealthInstance;

    [EventRef]
    public string GameplayMusic;
    public EventInstance GameplayMusicInstance;

    [EventRef]
    public string SpeechIntro;
    public EventInstance SpeechIntroInstance;

    [EventRef]
    public string Speech1;
    public EventInstance Speech1Instance;

    [EventRef]
    public string Speech2;
    public EventInstance Speech2Instance;

    [EventRef]
    public string FinalDialogue;
    public EventInstance FinalDialogueInstance;

    public void Initialize()
    {
        MomStartsDying = false;

        SnoringInstance = RuntimeManager.CreateInstance(SnoringSound);
        RechargeInstance = RuntimeManager.CreateInstance(RechargeSound);
        LowHealthInstance = RuntimeManager.CreateInstance(LowHealthSound);
        FinalDialogueInstance = RuntimeManager.CreateInstance(FinalDialogue);


        GameplayMusicInstance = RuntimeManager.CreateInstance(GameplayMusic);
        SpeechIntroInstance = RuntimeManager.CreateInstance(SpeechIntro);
        Speech1Instance = RuntimeManager.CreateInstance(Speech1);
        Speech2Instance = RuntimeManager.CreateInstance(Speech2);
    }
}
