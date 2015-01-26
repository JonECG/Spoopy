using UnityEngine;
using System.Collections;

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
        first = Instantiate(baseRoom, Vector3.zero, Quaternion.identity) as GameObject;
        second = Instantiate(baseRoom, Vector3.zero, Quaternion.identity) as GameObject;

        first.GetComponent<Room>().createRoom(5, 3, roomGen);
        second.GetComponent<Room>().createRoom(5, 5, roomGen);
        second.GetComponent<Room>().placeGenRoom(first.GetComponent<Room>().doors[0]);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
