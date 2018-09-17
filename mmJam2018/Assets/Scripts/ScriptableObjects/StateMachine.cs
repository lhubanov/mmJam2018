using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.States;

using FMODUnity;
using FMOD.Studio;

[CreateAssetMenu(menuName="World State Machine")]
public class StateMachine : ScriptableObject
{
    public IState CurrentState;

    // Health bar references
    //TODO: Add

    // Audio references
    [EventRef]
    public string SnoringSound;
    public EventInstance SnoringInstance;

    [EventRef]
    public string RechargeSound;
    public EventInstance RechargeInstance;

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

    public void Initialize()
    {
        SnoringInstance = RuntimeManager.CreateInstance(SnoringSound);

        RechargeInstance = RuntimeManager.CreateInstance(RechargeSound);
        GameplayMusicInstance = RuntimeManager.CreateInstance(GameplayMusic);
        SpeechIntroInstance = RuntimeManager.CreateInstance(SpeechIntro);
        Speech1Instance = RuntimeManager.CreateInstance(Speech1);
        Speech2Instance = RuntimeManager.CreateInstance(Speech2);
    }
}
