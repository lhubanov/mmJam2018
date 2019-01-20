using UnityEngine;
using Assets.Scripts;
using Assets.Scripts.States;

using FMODUnity;
using FMOD.Studio;

[CreateAssetMenu(menuName="World State Machine")]
public class StateMachine : ScriptableObject
{
    public IState   CurrentState;

    public delegate void EventHandler();
    public event EventHandler OnMomStartsDying;

    // UI references
    public float    HeldEnergy;
    public float    MomCurrentHealth;

    public float    MomMaxHealth;
    public float    MomMinHealth;

    public float    PlayerMovementSlowdown;

    // The unity UI image component's transparency overflows,
    // so this is just to make sure screen is black, and does not 
    // become transparent again.
    [SerializeField]
    private float fadeAmount;
    public float FadeAmount
    {
        get { return fadeAmount; }

        set
        {
            if(value > 255) {
               value = 255;
            }

            fadeAmount = value;
        }
    }

    public TileLookup TileLookup;

    public EventInstance CurrentMusic;
    public EventInstance CurrentDialogue;

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
    public string MenuMusic;
    public EventInstance MenuMusicInstance;

    [EventRef]
    public string GameplayMusic;
    public EventInstance GameplayMusicInstance;

    [EventRef]
    public string EndingMusic;
    public EventInstance EndingMusicInstance;

    [EventRef]
    public string SpeechIntro;
    public EventInstance SpeechIntroInstance;

    [EventRef]
    public string Speech1;
    public EventInstance Speech1Instance;

    [EventRef]
    public string FinalDialogue;
    public EventInstance FinalDialogueInstance;

    public void Initialize()
    {
        FadeAmount = 0;

        TileLookup.Initialize();

        SnoringInstance = RuntimeManager.CreateInstance(SnoringSound);
        RechargeInstance = RuntimeManager.CreateInstance(RechargeSound);
        LowHealthInstance = RuntimeManager.CreateInstance(LowHealthSound);
        FinalDialogueInstance = RuntimeManager.CreateInstance(FinalDialogue);
        EndingMusicInstance = RuntimeManager.CreateInstance(EndingMusic);

        MenuMusicInstance = RuntimeManager.CreateInstance(MenuMusic);
        GameplayMusicInstance = RuntimeManager.CreateInstance(GameplayMusic);
        SpeechIntroInstance = RuntimeManager.CreateInstance(SpeechIntro);
        Speech1Instance = RuntimeManager.CreateInstance(Speech1);
    }

    public void MomStartsDying()
    {
        if(OnMomStartsDying != null) {
            OnMomStartsDying();
        }
    }
}
