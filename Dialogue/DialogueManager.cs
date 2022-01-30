using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    #region Variable Declaration
    public Text dialogueText; //References to where the text appears
    public Text nameText;

    public Animator dialogueAnimator; //These are used to make the text boxes
    public Animator nameAnimator; //Pop up and go away

    public Text responseText1; //These are for when I need
    public Text responseText2; //To display any responses 

    public Animator responseAnimator; //Animates the response UI object

    private Queue<string> sentences;
    private Queue<string> names;
    public int messageIndex = 0;

    private bool inResponse = false; //Used to make sure I don't accidentally initiate a convo multiple times

    private Dialogue dialogue;

    public delegate void DialogueEvents();
    public static event DialogueEvents OnStart;
    public static event DialogueEvents OnEnd;
    #endregion

    #region Event Subscription
    private void OnEnable()
    {
        Interactable.DisplaySomeText += StartDialogue;
        Interactable.OnNext += DisplayNextSentence;
    }
    private void OnDisable()
    {
        Interactable.DisplaySomeText -= StartDialogue;
        Interactable.OnNext -= DisplayNextSentence;
    }
    #endregion

    void Start()
    {
        sentences = new Queue<string>();
        names = new Queue<string>();
    }

    #region Start/End Dialogue
    public void StartDialogue(Dialogue newDialogue)
    {
        dialogue = newDialogue;

        dialogueAnimator.SetBool("IsOpen", true);

        if (OnStart != null)
        {
            OnStart(); //Freezes player in place
        }

        //Setup the name queue to display the right names.
        names.Clear();

        if (dialogue.displayName)
        {
            nameAnimator.SetBool("IsOpen", true);
            foreach (string name in dialogue.message[messageIndex].names)
            {
                names.Enqueue(name);
            }
        }

        //Setup the sentence queue to display all text.
        sentences.Clear();

        foreach (string sentence in dialogue.message[messageIndex].sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    void EndDialogue()
    {
        StopAllCoroutines(); //Prevent TypeSentence from adding more letters to box
        dialogueAnimator.SetBool("IsOpen", false);      //Hide boxes
        nameAnimator.SetBool("IsOpen", false);
        dialogueText.text = "";                         //Clear text boxes
        nameText.text = "";
        messageIndex = 0;                               //reset message index
        EndResponse();
        if (OnEnd != null) //Send end event so player can move.
        {
            OnEnd();
        }
    } //Well it uh, ends the dialogue
#endregion

    #region Response Functions
    public void DisplayResponse()
    {
        responseText1.text = dialogue.message[messageIndex].responses[0].reply;
        responseText2.text = dialogue.message[messageIndex].responses[1].reply;
        responseAnimator.SetBool("IsOpen", true);
    }

    public void OnChoice1()
    {
        messageIndex = dialogue.message[messageIndex].responses[0].next;
        if (messageIndex == -1)
        {
            sentences.Clear();
            names.Clear();
            EndDialogue();
        }
        else
        {
            sentences.Clear();
            names.Clear();

            foreach (string sentence in dialogue.message[messageIndex].sentences)
            {
                sentences.Enqueue(sentence);
            }

            EndResponse();
            DisplayNextSentence();
        }
    }

    public void OnChoice2()
    {
        messageIndex = dialogue.message[messageIndex].responses[1].next;
        if (messageIndex == -1)
        {
            sentences.Clear();
            names.Clear();
            EndDialogue();
        }
        else
        {
            sentences.Clear();
            names.Clear();

            foreach (string sentence in dialogue.message[messageIndex].sentences)
            {
                sentences.Enqueue(sentence);
            }

            EndResponse();
            DisplayNextSentence();
        }
    }

    public void EndResponse()
    {
        responseAnimator.SetBool("IsOpen", false);
        responseText1.text = "";
        responseText2.text = "";
        inResponse = false;
    }
    #endregion
    
    public void CheckForEnd()
    {
        if (sentences.Count == 0) 
        {//no more sentences, check if extra things to be done
            if (dialogue.message[messageIndex].hasResponse) //If there's a response, display it
            {
                DisplayResponse();
                inResponse = true;
            }
            else if (dialogue.message[messageIndex].triggersEvent) //If there's an event, start it
            {
                ToggleEvent();
                EndDialogue();
            }
            else //(all the above seems pretty self explanatory, tbh) Past me was an idiot holy shit
            {
                EndDialogue();
            }
        }
    }

    public void DisplayNextSentence()
    {
        CheckForEnd(); //Make sure to end dialogue, or display response

        if (dialogue.displayName && !inResponse && messageIndex != -1 && names.Count != 0) //Display a name
        {
            string nameToDisplay = names.Dequeue();
            if (nameToDisplay == null)
            {
                nameText.text = "";
            }
            else
            {
                nameText.text = nameToDisplay;
            }
        }

        if (!inResponse && messageIndex != -1 && sentences.Count != 0)
        {
            string sentence = sentences.Dequeue();  //All this just displays text
            StopAllCoroutines();                    //if there is still stuff to display
            StartCoroutine(TypeSentence(sentence));
        }
    }

    private void ToggleEvent()
    {
        //So this is maybe temporary, it's not a fantastic implementation (I think, I'd need someone smarter than me to check that)
        //Gets name of whatever gameObject is needed with an attached GameEvent script
        //and uses the .Find() function to locate it. It then calls that scripts ToggleEvent() to start its event
        string gObject = dialogue.message[messageIndex].eventObject;
        GameEvent daEvent = GameObject.Find(gObject).GetComponent<GameEvent>();
        daEvent.ToggleEvent();
    }

    IEnumerator TypeSentence(string sentence)
    {
        //Does a fancy typewriter effect by converting the sentence into
        //a character array, wherein each letter is individually added into the text box
        //it looks super neat! I should mess with variable typing speeds.
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    } 
}
