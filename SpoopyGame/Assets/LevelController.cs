using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelController : MonoBehaviour {
    public int numberOfRooms;
    public int maxRoomSize;
    public GameObject baseRoom;
    Random rand = new Random();
    RoomGeneratorScript roomGen;
    GameObject first;
    GameObject second;

	// Use this for initialization
	void Start () {
        roomGen = this.GetComponent<RoomGeneratorScript>();
        randomFirstroom();
        genreateMap();


	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void randomFirstroom()
    {
        first = Instantiate(baseRoom, Vector3.zero, Quaternion.identity) as GameObject;
        int randomRoomSizeX = getRandomOddValueInSize();
        int randomRoomSizeY = getRandomOddValueInSize();
        first.GetComponent<Room>().createRoom(randomRoomSizeX, randomRoomSizeY, roomGen);
    }

    void genreateMap()
    {
        List<Door> firstRoomsdoors = first.GetComponent<Room>().getDoors();
        foreach (Door d in firstRoomsdoors)
        {
            int randomRoomSizeX = getRandomOddValueInSize();
            int randomRoomSizeY = getRandomOddValueInSize();
            GameObject newRoom = Instantiate(baseRoom, Vector3.zero, Quaternion.identity) as GameObject;
            newRoom.GetComponent<Room>().createRoom(randomRoomSizeX, randomRoomSizeY, roomGen);
            newRoom.GetComponent<Room>().placeGenRoom(d);
        }
    }

    private int getRandomOddValueInSize()
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
}
