using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DummyDoor
{
    public DummyDoor connectedDoor;
    public DummyRoom connectedRoom;
    public int accessibilityMask;
}

public class DummyRoom
{
    List<DummyDoor> doors;
}

public class RoomTraversal : MonoBehaviour {

	void Start () 
	{
	
	}
	
	void Update () 
	{
	
	}
}
