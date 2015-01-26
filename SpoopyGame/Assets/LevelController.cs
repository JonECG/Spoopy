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
        second = Instantiate(baseRoom, Vector3.zero, Quaternion.identity) as GameObject;

        first.GetComponent<Room>().createRoom(5, 3, roomGen);
        second.GetComponent<Room>().createRoom(5, 5, roomGen);
        second.GetComponent<Room>().placeGenRoom(first.GetComponent<Room>().doors[0]);


	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void randomFirstroom()
    {
        first = Instantiate(baseRoom, Vector3.zero, Quaternion.identity) as GameObject;
    }

    void genreateMap()
    {
        List<Door> firstRoomsdoors = first.GetComponent<Room>().getDoors();
        foreach (Door d in firstRoomsdoors)
        {
            int randomRoomSizeX = getRandomOddValueInSize();
            int randomRoomSizeY = getRandomOddValueInSize();
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
