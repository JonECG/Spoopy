using UnityEngine;
using System.Collections;

public class DoorNode : Node {

    private Vector3 exitDirection;
    private GameObject room;
    public GameObject DirectionIndicator;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void connectToRoomCore(GameObject node) 
    {
        connections.Add(node);
        room = node;
    }

    public void connectToNextDoorNode(GameObject node)
    {
        connections.Add(node);
    }

    public void setExitDirection(Vector3 exitDirection)
    {
        this.exitDirection = exitDirection;
        this.transform.forward = exitDirection;
        DirectionIndicator.transform.position = DirectionIndicator.transform.position + exitDirection;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Vector3 playerDirection = this.transform.position - other.gameObject.transform.position;
            float dotProduct = Vector3.Dot(playerDirection.normalized, exitDirection.normalized);
            if (dotProduct > 0)
            {
                room.GetComponent<RoomNode>().setIsActive(true);
                //RoomVisualizerScript.weightDungeon(room);
                //RoomVisualizerScript.visualizeRooms(room);
            }
            else
            {
                room.GetComponent<RoomNode>().setIsActive(false);
            }
            
        }
    }
}
