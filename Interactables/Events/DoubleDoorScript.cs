
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleDoorScript : GameEvent
{
    public Transform door1;
    public Transform door2;

    public bool isOpen = false;

    [SerializeField]
    private Vector3 door1Open;
    [SerializeField]
    private Vector3 door2Open;
    private Vector3 door1Close;
    private Vector3 door2Close;

    private void Start()
    {
        door1Close = door1.position;
        door2Close = door2.position;

        door1Open = new Vector3(door1Close.x - 3, door1Close.y, door1Close.z);
        door2Open = new Vector3(door2Close.x + 3, door2Close.y, door2Close.z);
    }

    public override void ToggleEvent()
    {
        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    private void CloseDoor()
    {
        isOpen = false;
        door1.position = door1Close;
        door2.position = door2Close;
    }
    private void OpenDoor()
    {
        isOpen = true;
        door1.position = door1Open;
        door2.position = door2Open;
    }
}
