using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    public bool isLocked { get; set; }
    public Vector3 roomCenterDirection { get; set; }
    public Vector3 position { get; set; }
    private Room room { get; set; }
    private Door otherDoor { get; set; }
    public int sizeOfWall { get; set; }
    public bool used;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void setExitDirection(Vector3 exitDirection) {
        this.transform.forward = exitDirection;
    }

    public void setRoom(Room room)
    {
        this.room = room;
        
    }

    public Room getConnectedRoom()
    {
        return otherDoor.getRoom();
    }

    public Room getRoom()
    {
        return room;
    }

    public void connectDoor(Door other)
    {
        otherDoor = other;
        used = true;
    }

    public Vector3 getVectorToRoom()
    {
        roomCenterDirection = room.transform.position -  this.transform.position ;
        return roomCenterDirection;
    }
}
