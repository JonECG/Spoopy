using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour {


    public bool isPreGenreatedRoom { get; set; }
    public GameObject prebuiltRoom { get; set; }
    public List<Door> doors = new List<Door>();

    private bool hasPlayer { get; set; }
    private bool endRoom { get; set; }
    private int sizeX { get; set; }
    private int sizeZ { get; set; }
    private int sightWeight { get; set; }
    
	// Use this for initialization
	void Start () 
    {
        createRoom(3, 3, this.gameObject.GetComponent<RoomGeneratorScript>());
        //this.tag = "Room";
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
        BoxCollider roomColider = new BoxCollider();
        roomColider.center = this.transform.position;
        roomColider.transform.parent = this.transform;
        roomColider.size = new Vector3(sizeX, 0.0f, sizeZ);
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
            int exitAmount = Random.Range(1, 4);
            int exitsOpened = 0;
            while (exitsOpened <= exitAmount)
            {
                int exitSelect = Random.Range(0, 4);
                if (!exits[exitSelect])
                {
                    exits[exitSelect] = true;
                    exitsOpened++;
                }

            }
        }
        GameObject newRoom = roomGen.getRectRoom(sizeX, sizeZ, exits[0], exits[1], exits[2], exits[3]);
        newRoom.transform.position = new Vector3(-(sizeX / 2), 0.0f, -(sizeZ/2));
        
        if (exits[0])
        {
            //Door northDoor = new Door();
            //northDoor.transform.position = new Vector3(1.0f, 0.0f, 0.0f);
            //northDoor.setRoom(this);
            //northDoor.setExitDirection(northDoor.transform.position - this.transform.position);
            //northDoor.transform.parent = this.transform;
        }
        if (exits[1])
        {
            //Door eastDoor = new Door();
            //eastDoor.transform.position = new Vector3(0.0f, 0.0f, 1.0f);
            //eastDoor.setRoom(this);
            //eastDoor.setExitDirection(eastDoor.transform.position - this.transform.position);
            //eastDoor.transform.parent = this.transform;
        }
        if (exits[2])
        {
            //Door southDoor = new Door();
            //southDoor.transform.position = new Vector3(-1.0f, 0.0f, 0.0f);
            //southDoor.setRoom(this);
            //southDoor.setExitDirection(southDoor.transform.position - this.transform.position);
            //southDoor.transform.parent = this.transform;
        }
        if (exits[3])
        {
            //Door westDoor = new Door();
            //westDoor.transform.position = new Vector3(0.0f, 0.0f, -1.0f);
            //westDoor.setRoom(this);
            //westDoor.setExitDirection(westDoor.transform.position - this.transform.position);
            //westDoor.transform.parent = this.transform;
        }
        newRoom.transform.parent = this.transform;        
    }
}
