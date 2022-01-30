using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : MonoBehaviour
{
    public delegate void StartInteraction(Dialogue dialogue);
    public static event StartInteraction ForceInteraction;
    /*
    This is the base class for all things that need to have something happen
    When the player interacts with an interactable
    You need to make a unique script for each object that does something
    This allows each object to have it's own unique behavior when it is
    interacted with

    NOTE: have scripts INHERIT from this class if you want all of this functionality

    DoubleDoorScript.cs is left in the files as an example of what 
    the use of the functions look like, just in case you forgot I guess
    */

    /*
    Use this function when you need to make an object do multiple things
    such as a button opening, then closing a door when interacted with again
    */
    public virtual void ToggleEvent() {}

    /*
    If you need a GameEvent script to start a conversation, use this
    In other words, if an event triggers another piece of dialogue
    this function will do just that
    */
    protected void StartForceInteraction(Dialogue dialogue)
    {
        ForceInteraction?.Invoke(dialogue);
    }
}
