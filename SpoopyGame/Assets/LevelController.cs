using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LevelController : MonoBehaviour {
    public int numberOfRooms;
    public int maxRoomSize;
    public GameObject baseRoom;
    public GameObject player;
    public int seededValue;
    RoomGeneratorScript roomGen;
    GameObject first;

	void Start () {
        Random.seed = seededValue;
        roomGen = this.GetComponent<RoomGeneratorScript>();
        prebuildFirstRoom();
        genreateMap(numberOfRooms);
        RoomVisualizerScript.weightDungeon(first.GetComponent<Room>());
        RoomVisualizerScript.visualizeRooms(first.GetComponent<Room>());
        player.transform.position = first.transform.position + new Vector3(0.0f, 0.5f, 0.0f);
	}
	
	void Update () {
	
	}


    void prebuildFirstRoom()
    {
        RoomInfo room = CustomRooms.Rooms.Where(n => n.name == "StartingRoom").FirstOrDefault();
        first = Instantiate(room.gameObjectReference) as GameObject;
        first.transform.position = Vector3.zero;
        first.GetComponent<Room>().createRoom("StartingRoom");
        first.GetComponent<Room>().roomDepth = numberOfRooms;
    }

    void randomFirstroom()
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

    void genreateMap(int roomCount)
    {
        List<Door> firstRoomsdoors = first.GetComponent<Room>().getDoors();
        int distributionIndex = 0;
        int[] roomDistribution = getRoomDistributions(roomCount-1, firstRoomsdoors.Count);
        for (int i = 0; i < firstRoomsdoors.Count; i++)
        {
            int randomRoomSizeX = getRandomOddValueInSize(first.GetComponent<Room>().sizeX);
            int randomRoomSizeY = getRandomOddValueInSize();
            GameObject newRoom = Instantiate(baseRoom, Vector3.zero, Quaternion.identity) as GameObject;
            newRoom.GetComponent<Room>().roomDepth = roomDistribution[distributionIndex];
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
            newRoom.GetComponent<Room>().placeGenRoom(firstRoomsdoors[i], randomRoomSizeX);
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
                    GameObject newRoom = Instantiate(baseRoom, Vector3.zero, Quaternion.identity) as GameObject;
                    newRoom.GetComponent<Room>().roomDepth = roomDistribution[distributionIndex];
                    int numDoors = 1;
                    if (roomDistribution[distributionIndex] == 1)
                    {
                        newRoom.GetComponent<Room>().endRoom = true;
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
                    newRoom.GetComponent<Room>().createRoom(randomRoomSizeX, randomRoomSizeY, numDoors, roomGen);
                    newRoom.GetComponent<Room>().placeGenRoom(firstRoomsdoors[i], randomRoomSizeX);
                    genreateRooms(newRoom, roomDistribution[distributionIndex]);
                    distributionIndex++;
                }
            }
        }
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
