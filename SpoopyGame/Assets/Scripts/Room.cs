using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour {

    public bool isPreGenreatedRoom { get; set; }

    public GameObject prebuiltRoom { get; set; }
    public GameObject baseDoor;

    public List<Door> doors = new List<Door>();

    private bool hasPlayer { get; set; }
    public int sightWeight { get; set; }
    public bool endRoom { get; set; }
    public int sizeX;
    public int sizeZ;
    public int roomDepth;
    public bool isPlayerRoom = false;
	// Use this for initialization
	void Start () 
    {
        
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (sightWeight <= 1)
        {
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
	}

    public int getWeight()
    {
        return sightWeight;
    }

    public void setWeight(int newWeight)
    {
        sightWeight = newWeight;
    }

    public void placeGenRoom(Door connectingDoor, int wallSize)
    {
        int doorSelection = getDoorWithWalSize(wallSize);
        Vector3 connectingDoorExit = connectingDoor.transform.forward.normalized;
        Vector3 thisDoosExit = doors[doorSelection].transform.forward.normalized;
        float angle = Vector3.Angle(thisDoosExit, connectingDoorExit);
        rotateRoom(angle);

        float dot = Vector3.Dot(connectingDoor.transform.forward.normalized, doors[doorSelection].transform.forward.normalized);
        
        if (dot != -1.0f)
        {
            rotateRoom(180);
        }
        
        this.transform.position = connectingDoor.transform.position + doors[doorSelection].getVectorToRoom();

        doors[doorSelection].connectDoor(connectingDoor);
        connectingDoor.connectDoor(doors[doorSelection]);
    }

    private void rotateRoom(float angle)
    {
        this.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), -angle);
    }

    public void createRoom(int x, int z, int numDoors, RoomGeneratorScript roomGen)
    {
        setSize(x, z);
        if(isPreGenreatedRoom)
        {

        }
        else 
        {
            genreateRoom(roomGen, numDoors);
        }
        createRoomTrigger();
    }

    private void setSize(int x, int z) 
    {
        sizeX = x;
        sizeZ = z;
    }

    private void addDoor(Door newDoor)
    {
        doors.Add(newDoor);
    }

    private void createRoomTrigger()
    {
        this.gameObject.AddComponent("BoxCollider");
        BoxCollider roomColider = this.GetComponent<BoxCollider>();
        roomColider.center = this.transform.position;
        roomColider.transform.parent = this.transform;
        roomColider.size = new Vector3((sizeX * 2) - 0.001f, 1.0f, (sizeZ * 2) - 0.001f);
        roomColider.isTrigger = true;
        
    }

    private void genreateRoom(RoomGeneratorScript roomGen, int numDoors)
    {
        bool[] exits = new bool[4];
        if (endRoom)
        {
            exits[0] = true;
        }
        else
        {
            int exitAmount = numDoors;
            for (int i = 0; i < exitAmount; i++)
            {
                exits[i] = true;
            }
        }
        GameObject newRoom = roomGen.getRectRoom(sizeX, sizeZ, exits[3], exits[0], exits[1], exits[2]);
        newRoom.transform.position = new Vector3(-(sizeX / 2), 0.0f, -(sizeZ/2));
        
        if (exits[0])
        {
            GameObject newDoor = Instantiate(baseDoor, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;
            Door northDoor = newDoor.GetComponent<Door>();
            newDoor.transform.position = new Vector3(1.0f, 0.0f, 0.0f) * (sizeX);
            northDoor.setRoom(this);
            northDoor.setExitDirection(northDoor.transform.position - this.transform.position);
            northDoor.sizeOfWall = sizeX;
            newDoor.transform.parent = this.transform;
            addDoor(northDoor);
        }
        if (exits[1])
        {
            GameObject newDoor = Instantiate(baseDoor, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;
            Door eastDoor = newDoor.GetComponent<Door>();
            newDoor.transform.position = new Vector3(0.0f, 0.0f, 1.0f) * (sizeZ);
            eastDoor.setRoom(this);
            eastDoor.setExitDirection(eastDoor.transform.position - this.transform.position);
            eastDoor.sizeOfWall = sizeZ;
            newDoor.transform.parent = this.transform;
            addDoor(eastDoor);
        }
        if (exits[2])
        {
            GameObject newDoor = Instantiate(baseDoor, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;
            Door southDoor = newDoor.GetComponent<Door>();
            newDoor.transform.position = new Vector3(-1.0f, 0.0f, 0.0f) * (sizeX);
            southDoor.setRoom(this);
            southDoor.setExitDirection(southDoor.transform.position - this.transform.position);
            southDoor.sizeOfWall = sizeX;
            newDoor.transform.parent = this.transform;
            addDoor(southDoor);
        }
        if (exits[3])
        {
            GameObject newDoor = Instantiate(baseDoor, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;
            Door westDoor = newDoor.GetComponent<Door>();
            newDoor.transform.position = new Vector3(0.0f, 0.0f, -1.0f) * (sizeZ);
            westDoor.setRoom(this);
            westDoor.setExitDirection(westDoor.transform.position - this.transform.position);
            westDoor.sizeOfWall = sizeZ;
            newDoor.transform.parent = this.transform;
            addDoor(westDoor);
        }
        newRoom.transform.parent = this.transform;        
    }

    public List<Door> getDoors()
    {
        return doors;
    }

    public bool checkOverlap(Bounds otherBounds)
    {
        return this.gameObject.GetComponent<BoxCollider>().bounds.Intersects(otherBounds);
    }

    public int getDoorWithWalSize(int wallSize)
    {
        bool foundDoorOfSize = false;
        int foundDoor = -1;
        for (int i = 0; i < doors.Count && !foundDoorOfSize; i++)
        {
            if (doors[i].sizeOfWall == wallSize)
            {
                foundDoorOfSize = true;
                foundDoor = i;
            }
        }
        return foundDoor;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("TriggerEntered");
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("TriggerEntered by player");
            RoomVisualizerScript.weightDungeon(this);
            RoomVisualizerScript.visualizeRooms(this);
        }
    }
}
