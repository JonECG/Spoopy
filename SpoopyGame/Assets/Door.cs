using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    public bool isLocked { get; set; }
    public Vector3 roomCenterDirection { get; set; }
    public Vector3 position { get; set; }
    public Room room;
    public Door otherDoor;
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
        other.otherDoor = (this);
        used = true;
    }

    public Vector3 getVectorToRoom()
    {
        roomCenterDirection = room.transform.position -  this.transform.position ;
        return roomCenterDirection;
    }

    public void addCollider()
    {
        this.gameObject.AddComponent<BoxCollider>();
        this.gameObject.GetComponent<BoxCollider>().size = new Vector3(1.0f, 1.0f, 1.0f);
        this.gameObject.GetComponent<BoxCollider>().isTrigger = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Vector3 playerPos = this.transform.position - other.transform.position;
            float dot = Vector3.Dot(playerPos.normalized, this.transform.forward.normalized);
            if (dot > 0.0f)
            {
                RoomVisualizerScript.weightDungeon(this.room);
                RoomVisualizerScript.visualizeRooms(this.room);
            }
        }
    }
}
