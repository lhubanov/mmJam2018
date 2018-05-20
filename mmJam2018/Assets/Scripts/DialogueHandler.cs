using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHandler : MonoBehaviour {

    //loads right bits of dialogue @ start
    //shows right bits of dialogue @ trigger

    // does this need to be global @ static or do we just load all the dialogue at the start
    // and keep it contained?
    // then the trigger script/object will need to send requests to this, hence keep track of
    // where we are with the story?4

    // we can probably make momma static and she will keep track of this herself,
    // maybe even send the dialog to display to this (call a method w/ string param)
    // and this will just display said dialogue

    // DialogBox script needs to handle:
    //      fade in animation
    //      text child's text property change & enable?
    //      fade out animation, when character leaves area or
    //          dialogue string contains (END ?)

    // DialogueHandler script needs to:
    //      keep track of progress (flags for narrative events)
    //      play respective voice clips (or do call to dialogBox script to display respective text)
    //      play animations or call methods of objects that play animations
    //      call method to play voice clip of other participant in dialogue (player), after mom's dialogue is finished

    // If no written dialogue is needed

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
