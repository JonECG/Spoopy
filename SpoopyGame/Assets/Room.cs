using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour {


    public bool isPreGenreatedRoom { get; set; }
    public GameObject prebuiltRoom { get; set; }
    public GameObject baseDoor;
    public List<Door> doors = new List<Door>();

    private bool hasPlayer { get; set; }
    private bool endRoom { get; set; }
    private int sizeX { get; set; }
    private int sizeZ { get; set; }
    private int sightWeight { get; set; }
    
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

    public void placeGenRoom(Door connectingDoor)
    {
        int doorSelection = Random.Range(0, doors.Count);
        Vector3 connectingDoorExit = connectingDoor.exitDirection.normalized;
        Vector3 thisDoosExit = doors[doorSelection].exitDirection.normalized;
        float angle = Vector3.Angle(thisDoosExit, -connectingDoorExit);
        this.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), -angle);
        this.transform.position = connectingDoor.transform.position + doors[doorSelection].getVectorToRoom();
        doors[doorSelection].connectDoor(connectingDoor);
        connectingDoor.connectDoor(doors[doorSelection]);
    }

    public void createRoom(int x, int z, RoomGeneratorScript roomGen)
    {
        setSize(x, z);
        if(isPreGenreatedRoom)
        {

        }
        else 
        {
            genreateRoom(roomGen);
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

    private void genreateRoom(RoomGeneratorScript roomGen)
    {
        bool[] exits = new bool[4];
        if (endRoom)
        {
            exits[0] = true;
        }
        else
        {
            int exitAmount = Random.Range(2, 5);
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
            newDoor.transform.parent = this.transform;
            addDoor(northDoor);
            Debug.DrawLine(newDoor.transform.position, northDoor.exitDirection, Color.green);
        }
        if (exits[1])
        {
            GameObject newDoor = Instantiate(baseDoor, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;
            Door eastDoor = newDoor.GetComponent<Door>();
            newDoor.transform.position = new Vector3(0.0f, 0.0f, 1.0f) * (sizeZ);
            eastDoor.setRoom(this);
            eastDoor.setExitDirection(eastDoor.transform.position - this.transform.position);
            newDoor.transform.parent = this.transform;
            addDoor(eastDoor);
            Debug.DrawLine(newDoor.transform.position, eastDoor.exitDirection, Color.green);
        }
        if (exits[2])
        {
            GameObject newDoor = Instantiate(baseDoor, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;
            Door southDoor = newDoor.GetComponent<Door>();
            newDoor.transform.position = new Vector3(-1.0f, 0.0f, 0.0f) * (sizeX);
            southDoor.setRoom(this);
            southDoor.setExitDirection(southDoor.transform.position - this.transform.position);
            newDoor.transform.parent = this.transform;
            addDoor(southDoor);

            Debug.DrawLine(newDoor.transform.position, southDoor.exitDirection, Color.green);
        }
        if (exits[3])
        {
            GameObject newDoor = Instantiate(baseDoor, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;
            Door westDoor = newDoor.GetComponent<Door>();
            newDoor.transform.position = new Vector3(0.0f, 0.0f, -1.0f) * (sizeZ);
            westDoor.setRoom(this);
            westDoor.setExitDirection(westDoor.transform.position - this.transform.position);
            newDoor.transform.parent = this.transform;
            addDoor(westDoor);

            Debug.DrawLine(newDoor.transform.position, westDoor.exitDirection, Color.green);
        }
        newRoom.transform.parent = this.transform;        
    }
}
