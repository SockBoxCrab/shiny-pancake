using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue : ScriptableObject
{
    [Tooltip("Check this box if you want to display the interactable's name.")]
    public bool displayName;
    

    public Message[] message;
}
