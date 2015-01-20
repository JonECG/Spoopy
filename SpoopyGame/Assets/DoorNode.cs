using UnityEngine;
using System.Collections;

public class DoorNode : Node {


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void connectToRoomCore(GameObject node) 
    {
        connections.Add(node);
    }

    public void connectToNextDoorNode(GameObject node)
    {
        connections.Add(node);
    }
}
