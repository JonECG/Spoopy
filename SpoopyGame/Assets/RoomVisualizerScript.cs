using UnityEngine;
using System.Collections;

public class RoomVisualizerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
    void Update() { }

    public static void visualizeRooms(Room startRoom)
    {
        if (startRoom.getWeight() == 0)
        {
            startRoom.isPlayerRoom = true;
        }
        else
        {
            startRoom.isPlayerRoom = false;
        }
        for (int i = 0; i < startRoom.doors.Count; i++)
        {
            if (startRoom.doors[i].getConnectedRoom().getWeight() == 0)
            {
                startRoom.doors[i].getConnectedRoom().gameObject.SetActive(true);
            }
            else
            {
                startRoom.doors[i].getConnectedRoom().gameObject.SetActive(false);
            }
            visualizeSubRooms(startRoom, startRoom.doors[i].getConnectedRoom());
        }
    }

    private static void visualizeSubRooms(Room lastRoom, Room targetRoom)
    {
        for (int i = 0; i < targetRoom.doors.Count; i++)
        {
            if (targetRoom.doors[i].getConnectedRoom() != lastRoom)
            {
                if (targetRoom.doors[i].getConnectedRoom().getWeight() == 0)
                {
                    targetRoom.doors[i].getConnectedRoom().gameObject.SetActive(true);
                }
                else
                {
                    targetRoom.doors[i].getConnectedRoom().gameObject.SetActive(false);
                }
                visualizeSubRooms(targetRoom, targetRoom.doors[i].getConnectedRoom());
            }
        }
    }

    public static void weightDungeon(Room activeRoom)
    {
        activeRoom.sightWeight = 0;
        for (int i = 0; i < activeRoom.doors.Count; i++)
        {
            activeRoom.doors[i].getConnectedRoom().sightWeight = 0;
            Vector3 direction = activeRoom.transform.position - activeRoom.doors[i].getConnectedRoom().transform.position;
            weightRooms(activeRoom.doors[i].getConnectedRoom(), activeRoom, direction.normalized, 0);
        }
    }

    private static void weightRooms(Room activeRoom, Room lastroom, Vector3 lastDirection, int lastWeight)
    {
        for (int i = 0; i < activeRoom.doors.Count; i++)
        {
            if (!(activeRoom.doors[i].getConnectedRoom() == lastroom))
            {
                int weight = lastWeight;
                Vector3 direction = activeRoom.transform.position - activeRoom.doors[i].getConnectedRoom().transform.position;
                if (!(Vector3.Dot(direction.normalized, lastDirection.normalized) > 0.8f))
                {
                    weight = weight + 1;
                }
                activeRoom.doors[i].getConnectedRoom().setWeight(weight);
                weightRooms(activeRoom.doors[i].getConnectedRoom(), activeRoom, lastDirection.normalized, weight);
            }
        }
    }
}
