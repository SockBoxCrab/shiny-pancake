using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Generic movement/interaction script used for the text system

The only real necessary part to make this system work is a way
to fire off the OnStart and OnEnd events in the DialogueManager script
these two events begin/end "conversations" that appear in the text boxes
all of the other events, such as OnInteract and 
*/

/*
Note to self: change it so that advancing text in a conversation
is independent of the interactable script
right now the interactable checks when the player presses e
instead of the dialogue manager checking, which could lead to some
interesting cases
simplest way to do this is to make it so that this script
fires off a seperate event to the DialogueManager that advances the text
which I believe is the OnNext event in the Interactable script
have the Playerscript fire off an event that calls the DisplayNextSentence function
instead of having the Interactble script doing that
*/

/*
find way for dialogue to force quit if the object (somehow)
flies becomes out of distance for the character
*/

public class PlayerScript : MonoBehaviour
{
    #region Variable Declaration
    //Event variables
    public delegate void InteractWithItem(bool inInteract);
    public static event InteractWithItem OnInteract; 
    [SerializeField]
    private CharacterController player;
    //movement vars
    public float speed;
    public float gravScale;
    //Restrictive Booleans
    //public bool isFalling;
    public bool inDialogue = false;
    #endregion

    #region Event Subscription
    private void OnEnable()
    {
        DialogueManager.OnStart += StartDialogue;
        DialogueManager.OnEnd += EndDialogue;
    }
    private void OnDisable()
    {
        DialogueManager.OnStart -= StartDialogue;
        DialogueManager.OnEnd -= EndDialogue;
    }
    #endregion

    private void Start()
    {
        player = GetComponent<CharacterController>();
    }

    //The loop here is, Move player if not in dialogue
    //Check if player is trying to interact with something
    void Update() 
    {
        if (!inDialogue)
        {
            MovePlayer();
        }
        CheckForInteraction();
    }

    #region Movement
    private void MovePlayer()
    {
        float horiInput = Input.GetAxis("Horizontal"); //for x axis
        float vertInput = Input.GetAxis("Vertical"); //for z axis
        Vector3 moveDir = new Vector3(1 * horiInput * speed * Time.deltaTime, 0, 
            1 * vertInput * speed * Time.deltaTime);

        player.Move(moveDir);

        ApplyGravity();
    }

    //Probably need to change this later
    //Change it to acceleration, so it looks nicer
    //Make sure it approaches a relatively slow speed
    private void ApplyGravity() 
    {
        if (!player.isGrounded)
        {
            Vector3 gravity = new Vector3(0, Physics.gravity.y * gravScale, 0);
            player.Move(gravity);
        }
    }
    #endregion

    //If E key is pressed, tell subscribed objects
    //that the player is trying to interact with them
    //Will only do something if player is close enough
    //I need to figure out what to do if two objects are too close to each other 
    //Which one is interacted with? 
    private void CheckForInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (OnInteract != null)
            {
                OnInteract(inDialogue);
            }
        }
    }
    
    private void StartDialogue()
    {
        inDialogue = true;
    }
    private void EndDialogue()
    {
        inDialogue = false;
    }
}
