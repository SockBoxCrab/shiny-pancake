using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Message 
{
    public bool triggersEvent;
    public string eventObject;

    [TextArea(3, 10)]
    public string[] sentences; //text to be displayed upon initial interaction

    [Tooltip("Make sure to put a name for each sentence in the Dialogue!")]
    [TextArea(1,1)]
    public string[] names; //Names for each individual sentence, you'll have to just deal with inputting the name for each sentence.

    [Tooltip("Ensures the dialogue manager checks for any responses!")]
    public bool hasResponse; //If this is false, end the interaction
    public Response[] responses;
}
