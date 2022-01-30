using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Response
{
    [Tooltip("Set this to the message index that the response leads to!")]
    public int next; //If -1, then exit interaction
    public string reply;
}
