using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
The Button and Testing Sphere are left in this folder as examples of 
how this system works. These will likely become out of date
as I figure out how to make custom editor UI
*/

public class Interactable : MonoBehaviour
{
    #region Variable Declaration
    //Display Text Event
    public delegate void DisplayText(Dialogue textToDisplay);
    public static event DisplayText DisplaySomeText;
    public delegate void DisplayNext();
    public static event DisplayNext OnNext;
    //Text attached to any given interactable
    public Dialogue dialogue;
    //Vars for indicating interactablility
    public GameObject icon;
    public bool nearPlayer = false;
    public bool noIcon = false;
    #endregion

    //For Interactables, you need to make sure to subscribe to the OnInteract Event!
    //I probably don't need this info since this is a "generic" script that I attach to whatever
    //But just incase I need to make anything unique!
    //FYI, objects need a Unity Collision thingy for this to work
    #region Event Subscription
    private void OnEnable()
    {
        PlayerScript.OnInteract += InteractedWith;
        GameEvent.ForceInteraction += OnDisplay;
    }
    private void OnDisable()
    {
        PlayerScript.OnInteract -= InteractedWith;
        GameEvent.ForceInteraction -= OnDisplay;
    }
    #endregion

    //Display icon if player is near
    //This has to be tracked at all times
    //That's why it's in Update() :)
    private void Update()
    {
        ToggleVisibilty(); 
    }

    //This one is pretty self explanatory
    //But, when the player is close tot he object based on 
    //Unity collider shenanigans, show the icon
    //Icon is placed manually in the editor
    private void ToggleVisibilty()
    {
        if (!noIcon)
        {
            if (nearPlayer)
            {
                icon.SetActive(true);
            }
            else
            {
                icon.SetActive(false);
            }
        }
    }

    //Checks when player is near the object
    //used when deciding when the icon should show up
    #region Collision Checks
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            nearPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            nearPlayer = false;
        }
    }
    #endregion

    //Triggers the DisplayText event which begins text "conversations"
    //Basically, the dialogue ScriptableObject is sent to the DialogueManager
    //which then handles all the stuff for displaying the text and responses
    protected void OnDisplay(Dialogue textToDisplay)
    {
        DisplaySomeText?.Invoke(textToDisplay);
    }

    //Call the OnDisplay() function to start a conversation with the dialogue
    //attached to the object. If player is already in a "conversation" with object
    //Aadvance the text instead with the OnNext() function
    public virtual void InteractedWith(bool isInteracting)
    {
        if (nearPlayer && !isInteracting)
        {
            OnDisplay(dialogue);
        }
        else if (nearPlayer && isInteracting)
        {
            OnNext(); //we don't check if this one is subscribed because we wouldn't be at this point otherwise. Might be safe to check it anyways
        }
    }
}