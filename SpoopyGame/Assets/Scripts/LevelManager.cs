using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

    public GameObject mainNode;
    public GameObject doorNode;
    public GameObject player;
    public GameObject startingRoom;
    public GameObject landMark;
    public int maxPathDepth;
    public int maxRoomSize;
    private Random rand = new Random();
    private GameObject startingNode;
    private Vector3 northVector = new Vector3(1.0f, 0.0f, 0.0f);
    private Vector3 eastVector = new Vector3(0.0f, 0.0f, 1.0f);
    private Vector3 southhVector = new Vector3(-1.0f, 0.0f, 0.0f);
    private Vector3 westVector = new Vector3(0.0f, 0.0f, -1.0f);

	// Use this for initialization
	void Start () {
        genreateNodeMap(genreateStartRoom());
        startingNode.GetComponent<RoomNode>().setIsActive(true);
        player.transform.position = startingNode.transform.position;
        RoomVisualizerScript.weightDungeon(startingNode);
	}

	// Update is called once per frame
	void Update () {
	
	}

    private GameObject genreateStartRoom()
    {
        startingRoom.transform.position = Vector3.zero;
        RoomNode tempRoom = startingRoom.GetComponent<RoomNode>();
        tempRoom.setRoomSize(3);
        int numberOfExits = 1;
        tempRoom.openExit(0);
        spawnDoorNodes(startingRoom);
        startingNode = startingRoom;
        return startingRoom;
        //GameObject newRoomNode = Instantiate(mainNode, Vector3.zero, Quaternion.identity) as GameObject;
        //RoomNode tempRoom = newRoomNode.GetComponent<RoomNode>();
        //tempRoom.setRoomSize(genRoomSize());
        //int numberOfExits = Random.Range(1, 4);
        //Debug.Log(numberOfExits);
        //for (int i = 0; i < numberOfExits; i++)
        //{
        //    tempRoom.openExit(i);
        //}
        //tempRoom.SpawnRoom(this.GetComponent<RoomGeneratorScript>());
        //tempRoom.setIsActive(true);
        //spawnDoorNodes(newRoomNode);
        //startingNode = newRoomNode;
        //return newRoomNode;
    }

    private void OpenRandomRoomExits(RoomNode node)
    {
        for (int i = 0; i < 4; i++)
        {
            if (Random.Range(0, 100) % 2 == 0)
            {
                node.openExit(i);
            }
        }
    }

    private int genRoomSize()
    {
        bool isOdd = false;
        int finalRoomSize = 0;
        while (!isOdd)
        {
            int genRoomSize = Random.Range(3, maxRoomSize);
            if (!(genRoomSize % 2 == 0))
            {
                isOdd = true;
                finalRoomSize = genRoomSize;
            }
        }
        return finalRoomSize;
    }

    public void genreateNodeMap(GameObject startingRoom)
    {
        RoomNode startingNode = startingRoom.GetComponent<RoomNode>();
        for (int i = 0; i < 4; i++)
        {
            if (startingNode.isOpen(i))
            {
                int newRoomSize = genRoomSize();
                Vector3 newRoomPosition = startingRoom.transform.position + (((newRoomSize + startingNode.getRoomSize())) * getDirectionVector(i));
                GameObject newRoomNode = Instantiate(mainNode, newRoomPosition, Quaternion.identity) as GameObject;
                RoomNode tempNewRoomNode = newRoomNode.GetComponent<RoomNode>();
                tempNewRoomNode.connectRoomNodes(startingRoom);
                tempNewRoomNode.openExit(openConnectingPath(i));
                tempNewRoomNode.setRoomSize(newRoomSize);
                int newDepth = 1;
                tempNewRoomNode.currentDepth = newDepth;
                if (!(newDepth >= maxPathDepth))
                {
                    OpenRandomRoomExits(tempNewRoomNode);
                }
                
                tempNewRoomNode.setParentRoomDirection(openConnectingPath(i));
                spawnDoorNodes(newRoomNode);
                tempNewRoomNode.SpawnRoom(this.GetComponent<RoomGeneratorScript>());
                genreateRooms(newDepth, newRoomNode);
            }
        }
    }

    private void genreateRooms(int currentDepth, GameObject lastRoom)
    {
        RoomNode lastNode = lastRoom.GetComponent<RoomNode>();
        if (!(currentDepth >= maxPathDepth))
        {
            for (int i = 0; i < 4; i++)
            {
                if (lastNode.isOpen(i) && i != lastNode.parentRoomDirection)
                {
                    int newRoomSize = genRoomSize();
                    Vector3 newRoomPosition = lastRoom.transform.position + (((newRoomSize + lastNode.getRoomSize())) * getDirectionVector(i));
                    GameObject newRoomNode = Instantiate(mainNode, newRoomPosition, Quaternion.identity) as GameObject;
                    RoomNode tempNewRoomNode = newRoomNode.GetComponent<RoomNode>();
                    tempNewRoomNode.connectRoomNodes(lastRoom);
                    tempNewRoomNode.openExit(openConnectingPath(i));
                    tempNewRoomNode.setRoomSize(newRoomSize);
                    int newDepth = currentDepth + 1;
                    tempNewRoomNode.currentDepth = newDepth;
                    if (!(newDepth >= maxPathDepth))
                    {
                        OpenRandomRoomExits(tempNewRoomNode);
                    }
                    tempNewRoomNode.setParentRoomDirection(openConnectingPath(i));
                    spawnDoorNodes(newRoomNode);
                    tempNewRoomNode.SpawnRoom(this.GetComponent<RoomGeneratorScript>());
                    if (Random.Range(0, 20) % 3 == 0)
                    {
                        tempNewRoomNode.createLandMark();
                    }
                    genreateRooms(newDepth, newRoomNode);
                }
            }
        }
    }

    public Vector3 getDirectionVector(int i)
    {
        Vector3 direction = Vector3.zero;
        switch (i)
        {
            case 0:
                direction = northVector;
                break;
            case 1:
                direction = eastVector;
                break;
            case 2:
                direction = southhVector;
                break;
            case 3:
                direction = westVector;
                break;
        }
        return direction;
    }

    private void spawnDoorNodes(GameObject core)
    {
        RoomNode node = core.GetComponent<RoomNode>();
        for (int i = 0; i < 4; i++)
        {
            if(node.isOpen(i))
            {
                Vector3 positionOfDoor = core.transform.position + (getDirectionVector(i) * (node.getRoomSize()));
                GameObject newDoor = Instantiate(doorNode, positionOfDoor, Quaternion.identity) as GameObject;
                newDoor.GetComponent<DoorNode>().setExitDirection(getDirectionVector(i));
                newDoor.transform.parent = core.transform;

                connectDoorAndRoom(core, newDoor);
            }
        }
    }

    private void connectDoorAndRoom(GameObject room, GameObject door)
    {
        RoomNode rNode = room.GetComponent<RoomNode>();
        DoorNode dNode = door.GetComponent<DoorNode>();
        rNode.connectDoor(door);
        dNode.connectToRoomCore(room);
    }

    private int openConnectingPath(int connectingDirection)
    {
        int direction = 0;
        switch (connectingDirection)
        {
            case 0:
                direction = 2;
                break;
            case 1:
                direction = 3;
                break;
            case 2:
                direction = 0;
                break;
            case 3:
                direction = 1;
                break;
        }
        return direction;
    }
}
