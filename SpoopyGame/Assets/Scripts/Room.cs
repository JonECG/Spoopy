using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class Room : MonoBehaviour {

    public bool isPreGenreatedRoom { get; set; }
    public GameObject prebuiltRoom { get; set; }
    public GameObject baseDoor;
    public List<Door> doors = new List<Door>();
    public bool useStartVector;
    public Vector3 startVector;
    private bool hasPlayer { get; set; }
    public int mark = 0;
    public int sightWeight { get; set; }
    public bool endRoom { get; set; }
    public int sizeX;
    public int sizeZ;
    public int roomDepth;
    public bool isPlayerRoom = false;
    public Vector3 startingPosition;
    public bool visualized { get; set; }
    public DoorFrequency freq;
	// Use this for initialization
	void Start () 
    {
        startingPosition = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {}

    void LateUpdate()
    {
        visualized = false;
    }


    public void setStartingPositoin(Vector3 position)
    {
        startingPosition = position;
        
    }

    public int getWeight()
    {
        return sightWeight;
    }

    public void setWeight(int newWeight)
    {
        sightWeight = newWeight;
    }

    public void placeGenRoom(Door connectingDoor, int wallSize, GameObject swingDoor, bool SelectFirst = false)
    {   
        int doorSelection = 0;
        if (!SelectFirst)
        {
            doorSelection = getDoorWithWalSize(wallSize);
        }
        
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
        startingPosition = this.transform.position;
        doors[doorSelection].connectDoor(connectingDoor);
        spawnDoors(swingDoor);
    }

    private void spawnDoors(GameObject swingDoor)
    {
        if (freq != DoorFrequency.Never)
        {
            for (int i = 0; i < doors.Count; i++)
            {
                bool makeDoor = true;
                if (freq == DoorFrequency.Normal)
                {
                    if (Random.value < 0.5f)
                    {
                        makeDoor = false;
                    }
                }
                if (!doors[i].used && makeDoor)
                {
                    Vector3 doorShiftedPosition = doors[i].transform.position;
                    GameObject newSwingDoor = Instantiate(swingDoor, doorShiftedPosition, Quaternion.identity) as GameObject;
                    newSwingDoor.transform.right = doors[i].transform.forward;
                    newSwingDoor.transform.parent = doors[i].transform;
                    ColorCodeValues keyColor = ColorCodeValues.White;
                    while (keyColor == ColorCodeValues.White)
                    {
                        keyColor = GetRandomColorEnum();
                    }
                    newSwingDoor.transform.FindChild("Door").GetComponent<SwingDoor>().color = keyColor;
                    if (Random.value < 0.3f)
                    {
                        newSwingDoor.transform.FindChild("Door").GetComponent<SwingDoor>().Locked = true;
                        
                    }
                }
            }
        }

    }

    private ColorCodeValues GetRandomColorEnum()
    {
        System.Array A = System.Enum.GetValues(typeof(ColorCodeValues));
        ColorCodeValues V = (ColorCodeValues)A.GetValue(UnityEngine.Random.Range(0, A.Length));
        return V;
    }

    private void rotateRoom(float angle)
    {
        this.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), -angle);
    }

    public void createRoom(string roomName)
    {
        createRoomTrigger();
        for (int i = 0; i < doors.Count; i++)
        {
            doors[i].setRoom(this);
        }
        setUpDoors();
    }

    public void createRoom(int x, int z, int numDoors, RoomGeneratorScript roomGen)
    {
        setSize(x, z);
        genreateRoom(roomGen, numDoors);
        createRoomTrigger();
        setUpDoors();
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
        roomColider.size = new Vector3((sizeX * 2) - 0.8f, 1.0f, (sizeZ * 2) - 0.8f);
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

    public void setUpDoors()
    {
        for (int i = 0; i < doors.Count; i++)
        {
            doors[i].addCollider();
        }
    }

    public void lockDoors(GameObject swingDoor)
    {
        for (int i = 0; i < doors.Count; i++)
        {
                Vector3 doorShiftedPosition = doors[i].transform.position;
                GameObject newSwingDoor = Instantiate(swingDoor, doorShiftedPosition, Quaternion.identity) as GameObject;
                newSwingDoor.transform.right = doors[i].transform.forward;
                newSwingDoor.transform.parent = doors[i].transform;
                newSwingDoor.transform.FindChild("Door").GetComponent<SwingDoor>().color = ColorCodeValues.White;
                newSwingDoor.transform.FindChild("Door").GetComponent<SwingDoor>().Locked = true;
        }
    }
}
