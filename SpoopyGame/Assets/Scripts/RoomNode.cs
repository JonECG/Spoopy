using UnityEngine;
using System.Collections;

public class RoomNode : Node {

    
    public bool[] exits = new bool[numExits];
    
    public int roomSize;
    public int parentRoomDirection;
    public int currentDepth;
    public bool active = false;
    public bool renderThis = false;
    private static int numExits = 4;
    private bool hall = false;

    // Use this for initialization
    void Start()
    {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void toggleActive()
    {
        active = !active;
    }

    public void toggleRender()
    {
        renderThis = !renderThis;
        this.active = renderThis;
    }

    public bool isOpen(int direction)
    {
        return exits[direction];
    }

    public void openExit(int targetExit)
    {
        exits[targetExit] = true;
    }

    public int getNumExits()
    {
        int numOpenExits = 0;
        for (int i = 0; i < numExits; i++)
        {
            if (exits[i])
            {
                numOpenExits++;
            }
        }
        return numOpenExits;
    }

    public void setRoomSize(int roomSize)
    {
        this.roomSize = roomSize;
    }

    public int getRoomSize()
    {
        return roomSize;
    }

    public void connectRoomNodes(GameObject nodeToConnectTo)
    {
        addConnection(nodeToConnectTo);
        RoomNode tempNode = nodeToConnectTo.GetComponent<RoomNode>();
        tempNode.addConnection(this.transform.gameObject);
    }

    public void setParentRoomDirection(int directionOfParentRoom)
    {
        parentRoomDirection = directionOfParentRoom;
    }

    public void SpawnRoom(RoomGeneratorScript roomGen)
    {
       GameObject newRoom = roomGen.getRectRoom(roomSize, roomSize, exits[3], exits[0], exits[1], exits[2]);
       Vector3 fixedPosition = new Vector3(this.transform.position.x - (int)(roomSize / 2), this.transform.position.y, this.transform.position.z - (int)(roomSize / 2));
       newRoom.transform.position = fixedPosition;
       newRoom.transform.parent = this.transform;
    }
}
