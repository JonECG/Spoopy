using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LevelController : MonoBehaviour {
    public int numberOfRooms;
    public int maxRoomSize;
    public GameObject baseRoom;
    public GameObject player;
    public int roomMod;
    public int seededValue;
    private bool finishPlaced;
    private bool firstLevel;
    private bool newLevel;

    private int odds;
    RoomGeneratorScript roomGen;
    GameObject first;
    GameObject LevelContainer;

	void Start () {
        
        firstLevel = true;
        CreateLevel();
        firstLevel = false;
        newLevel = false;
	}



    void Update()
    {
        if (newLevel)
        {
            
            CreateLevel();
        }
	}

    private void CreateLevel()
    {
        ClearDungeon();
        LevelContainer = new GameObject();
        LevelContainer.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        finishPlaced = false;
        Random.seed = seededValue + 37 * 5;
        roomGen = this.GetComponent<RoomGeneratorScript>();
        prebuildFirstRoom();
        genreateMap(numberOfRooms);
        RoomVisualizerScript.weightDungeon(first.GetComponent<Room>());
        RoomVisualizerScript.visualizeRooms(first.GetComponent<Room>());
        if (first.GetComponent<Room>().useStartVector)
        {
            player.transform.position = first.GetComponent<Room>().startVector;
        }
        else
        {
            player.transform.position = first.transform.position + new Vector3(0.0f, 0.5f, 0.0f);
        }
    }

    public void activateNewLevel()
    {
        newLevel = true;
    }

    private void ClearDungeon()
    {
        Destroy(LevelContainer);
    }
	
    private void prebuildFirstRoom()
    {
        RoomInfo room;
        if (firstLevel)
        {
            room = CustomRooms.Rooms.Where(n => n.name == "GameStartRoom").FirstOrDefault();
        }
        else
        {
            room = CustomRooms.Rooms.Where(n => n.name == "LevelStartRoom").FirstOrDefault();
        }
        first = Instantiate(room.gameObjectReference) as GameObject;
        first.transform.position = Vector3.zero;
        first.GetComponent<Room>().createRoom("GameStartRoom");
        first.GetComponent<Room>().roomDepth = numberOfRooms;
        first.gameObject.transform.SetParent(LevelContainer.transform);
    }

    private void randomFirstroom()
    {
        first = Instantiate(baseRoom, Vector3.zero, Quaternion.identity) as GameObject;
        int randomRoomSizeX = getRandomOddValueInSize();
        int randomRoomSizeY = getRandomOddValueInSize();
        int numDoorsFirstroom = 2;// Random.Range(2, 5);
        first.GetComponent<Room>().createRoom(randomRoomSizeX, randomRoomSizeY, numDoorsFirstroom, roomGen);
        first.GetComponent<Room>().roomDepth = numberOfRooms;
    }

    private int[] getRoomDistributions(int numbRooomsToSplit, int numDoors)
    {
        int[] roomDistribution = new int[numDoors];
        float[] ratios = new float[numDoors];
        float total = 0.0f;
        int roomsLeft = numbRooomsToSplit;
        for (int i = 0; i < numDoors; i++)
        {
            roomDistribution[i] += 1;
            ratios[i] = Random.value;
            total += ratios[i];
            roomsLeft--;
        }
        for (int i = 0; i < numDoors; i++ )
        {
            float percent = (ratios[i] / total) * 100;
            int numberRooms =(int)((percent / 100) * roomsLeft);
            roomDistribution[i] += numberRooms;
            roomsLeft -= numberRooms;
        }
        if (roomsLeft != 0 && numDoors != 0)
        {
            roomDistribution[0] += roomsLeft;
        }
        return roomDistribution;
    }

    private void genreateMap(int roomCount)
    {
        List<Door> firstRoomsdoors = first.GetComponent<Room>().getDoors();
        int distributionIndex = 0;
        int[] roomDistribution = getRoomDistributions(roomCount-1, firstRoomsdoors.Count);
        for (int i = 0; i < firstRoomsdoors.Count; i++)
        {
            int randomRoomSizeX = getRandomOddValueInSize(first.GetComponent<Room>().sizeX);
            int randomRoomSizeY = getRandomOddValueInSize();
            GameObject newRoom = Instantiate(baseRoom, Vector3.zero, Quaternion.identity) as GameObject;
            int numDoors = 1;
            if (roomDistribution[distributionIndex] == 1)
            {
                newRoom.GetComponent<Room>().endRoom = true;
            }
            else
            {
                if (roomDistribution[distributionIndex] == 2)
                {
                    numDoors = 2;
                }
                else
                {
                    numDoors = Random.Range(2, 5);
                }
            }
            newRoom.GetComponent<Room>().createRoom(randomRoomSizeX, randomRoomSizeY, numDoors, roomGen);
            newRoom.GetComponent<Room>().roomDepth = roomDistribution[distributionIndex];
            newRoom.GetComponent<Room>().placeGenRoom(firstRoomsdoors[i], randomRoomSizeX);
            newRoom.gameObject.transform.SetParent(LevelContainer.transform);
            genreateRooms(newRoom, roomDistribution[distributionIndex]);
            distributionIndex++;
        }
    }

    private void genreateRooms(GameObject sourceRoom, int roomCount)
    {
        if (roomCount >= 1)
        {
            List<Door> firstRoomsdoors = sourceRoom.GetComponent<Room>().getDoors();
            int distributionIndex = 0;
            int[] roomDistribution = getRoomDistributions(roomCount-1, firstRoomsdoors.Count - 1);
            for (int i = 0; i < firstRoomsdoors.Count; i++)
            {
                if (!firstRoomsdoors[i].used)
                {
                    int randomRoomSizeX = getRandomOddValueInSize(first.GetComponent<Room>().sizeX);
                    int randomRoomSizeY = getRandomOddValueInSize();
                    int numDoors = 1;
                    bool isAnEndRoom = false;
                    if (roomDistribution[distributionIndex] <= 1)
                    {
                        isAnEndRoom = true;
                        roomDistribution[distributionIndex] = 0;
                    }
                    else
                    {
                        if (roomDistribution[distributionIndex] == 2)
                        {
                            numDoors = 2;
                        }
                        else
                        {
                            numDoors = Random.Range(2, 5);
                        }

                    }
                    GameObject newRoom;
                    bool useFirstDoor = false;
                    Debug.Log("End" +isAnEndRoom);
                    bool mod = (odds % roomMod == 0);
                    Debug.Log(odds + roomMod + "Mod" + mod);
                    if (mod || isAnEndRoom)
                    {
                        if ((isAnEndRoom && !finishPlaced))
                        {
                            newRoom = createPremadeRoom(roomDistribution[distributionIndex], 1,  isAnEndRoom);
                            useFirstDoor = true;
                        }
                        else
                        {
                            newRoom = createPremadeRoom(roomDistribution[distributionIndex], numDoors, isAnEndRoom);
                            useFirstDoor = true;
                        }
                    }
                    else
                    {
                        newRoom = createRandomRoom(roomDistribution[distributionIndex], randomRoomSizeX, randomRoomSizeY, numDoors, isAnEndRoom);
                    }
                    newRoom.GetComponent<Room>().placeGenRoom(firstRoomsdoors[i], randomRoomSizeX, useFirstDoor);
                    newRoom.gameObject.transform.SetParent(LevelContainer.transform);
                    genreateRooms(newRoom, roomDistribution[distributionIndex]);
                    distributionIndex++;
                    odds++;
                }
            }
        }
    }

    private GameObject createRandomRoom( int depth, int x, int z, int doorCount, bool endRoom)
    {
        GameObject newRoom = Instantiate(baseRoom, Vector3.zero, Quaternion.identity) as GameObject;
        newRoom.GetComponent<Room>().roomDepth = depth;
        if (endRoom)
        {
            newRoom.GetComponent<Room>().endRoom = true;
        }
        newRoom.GetComponent<Room>().createRoom(x, z, doorCount, roomGen);
        return newRoom;
    }

    private GameObject createPremadeRoom(int depth, int numDoors, bool endRoom)
    {
        GameObject newRoom;
        if (!endRoom)
        {
            List<RoomInfo> room = CustomRooms.Rooms.Where(n => n.numOfDoors == numDoors).ToList();
            int randomSelection = Random.Range(0, room.Count);
            newRoom = Instantiate(room[randomSelection].gameObjectReference) as GameObject;
        }
        else
        {
            if (finishPlaced)
            {
                List<RoomInfo> room = CustomRooms.Rooms.Where(n => n.numOfDoors == 1 && (n.name != "FinishRoom" && n.name != "StartingRoom" && n.name != "GameStartRoom")).ToList();
                int randomSelection = Random.Range(0, room.Count);
                newRoom = Instantiate(room[randomSelection].gameObjectReference) as GameObject;
            }
            else
            {
                RoomInfo room = CustomRooms.Rooms.Where(n => n.name == "FinishRoom").FirstOrDefault();
                newRoom = Instantiate(room.gameObjectReference) as GameObject;
                finishPlaced = true;
            }
        }
        newRoom.transform.position = Vector3.zero;
        newRoom.GetComponent<Room>().createRoom("NewRoom");
        newRoom.GetComponent<Room>().roomDepth = depth;
        return newRoom;
    }

    private int getRandomOddValueInSize(int largest = 100)
    {
        largest = largest + 2;
        bool isOdd = false;
        int finalRoomSize = 0;
        while (!isOdd)
        {
            int maxSize = maxRoomSize;
            if (!(maxRoomSize < largest))
            {
                maxSize = largest;
            }
            int genRoomSize = Random.Range(3, maxSize);
            if (!(genRoomSize % 2 == 0))
            {
                isOdd = true;
                finalRoomSize = genRoomSize;
            }
        }
        return finalRoomSize;
    }
}
