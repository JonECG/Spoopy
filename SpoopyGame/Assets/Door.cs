using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    public bool isLocked { get; set; }
    public Vector3 exitDirection { get; set; }
    public Vector3 roomCenterDirection { get; set; }
    public Vector3 position { get; set; }
    private Room room { get; set; }
    private Door otherDoor { get; set; }
    
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void setExitDirection(Vector3 exitDirection) {
        this.exitDirection = exitDirection;
    }

    public void setRoom(Room room)
    {
        this.room = room;
        
    }

    public void connectDoor(Door other)
    {
        otherDoor = other;
    }

    public Vector3 getVectorToRoom()
    {
        roomCenterDirection = room.transform.position -  this.transform.position ;
        return roomCenterDirection;
    }
}
