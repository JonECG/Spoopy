using UnityEngine;
using System.Collections;

public class RoomVisualizerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
    void Update() { }

    public static void visualizeRooms(GameObject startRoom) 
    {
        if (startRoom.GetComponent<RoomNode>().getWeight() == 0)
        {
            startRoom.SetActive(true);
        }
        else
        {
            startRoom.SetActive(false);
        }
        for (int i = 0; i < startRoom.GetComponent<RoomNode>().getNumConnections(); i++)
        {
            if (startRoom.GetComponent<RoomNode>().connections[i].GetComponent<RoomNode>().getWeight() == 0)
            {
                startRoom.GetComponent<RoomNode>().connections[i].SetActive(true);
            }
            else
            {
                startRoom.GetComponent<RoomNode>().connections[i].SetActive(false);
            }
            visualizeSubRooms(startRoom, startRoom.GetComponent<RoomNode>().connections[i]);
        }
    }

    private static void visualizeSubRooms(GameObject lastRoom, GameObject targetRoom)
    {
        for (int i = 0; i < targetRoom.GetComponent<RoomNode>().getNumConnections(); i++)
        {
            if (targetRoom.GetComponent<RoomNode>().connections[i] != lastRoom)
            {
                if (targetRoom.GetComponent<RoomNode>().connections[i].GetComponent<RoomNode>().getWeight() == 0)
                {
                    targetRoom.GetComponent<RoomNode>().connections[i].SetActive(true);
                }
                else
                {
                    targetRoom.GetComponent<RoomNode>().connections[i].SetActive(false);
                }
                visualizeSubRooms(targetRoom, targetRoom.GetComponent<RoomNode>().connections[i]);
            }
        }
    }

    public static void weightDungeon(GameObject activeRoom)
    {
        activeRoom.GetComponent<RoomNode>().setWeight(0);
        for (int i = 0; i < activeRoom.GetComponent<RoomNode>().getNumConnections(); i++)
        {
            activeRoom.GetComponent<RoomNode>().connections[i].GetComponent<RoomNode>().setWeight(0);
            Vector3 direction = activeRoom.transform.position - activeRoom.GetComponent<RoomNode>().connections[i].transform.position;
            weightRooms(activeRoom.GetComponent<RoomNode>().connections[i], activeRoom, direction.normalized, 0);
        }
    }

    private static void weightRooms(GameObject activeRoom, GameObject lastroom, Vector3 lastDirection, int lastWeight)
    {
        for (int i = 0; i < activeRoom.GetComponent<RoomNode>().getNumConnections(); i++)
        {
            if (!(activeRoom.GetComponent<RoomNode>().connections[i] == lastroom))
            {
                int weight = lastWeight;
                Vector3 direction = activeRoom.transform.position - activeRoom.GetComponent<RoomNode>().connections[i].transform.position;
                if(!(Vector3.Dot(direction.normalized, lastDirection.normalized) > 0.8f))
                {
                    weight = weight + 1;
                }
                activeRoom.GetComponent<RoomNode>().connections[i].GetComponent<RoomNode>().setWeight(weight);
                weightRooms(activeRoom.GetComponent<RoomNode>().connections[i], activeRoom, lastDirection.normalized, weight);
            }
        }
    }
}
