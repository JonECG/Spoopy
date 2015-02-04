using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//public class DummyDoor
//{
//    public DummyDoor connectedDoor;
//    public DummyRoom connectedRoom;

//    private int _accessibilityMask;
//    public int AccessibilityMask
//    {
//        get
//        {
//            return _accessibilityMask | connectedDoor._accessibilityMask;
//        }
//        set
//        {
//            _accessibilityMask = value;
//            connectedDoor._accessibilityMask = value;
//        }
//    }
//}

//public class DummyRoom
//{
//    public List<DummyDoor> doors = new List<DummyDoor>();
//    public int accessibilityMask;
//    public int mark;
//    public string name;
//}

public class RoomTraversal : MonoBehaviour {

    //DummyRoom startRoom;

	void Start () 
	{
        //int RED_KEY = 1 << 0;
        //int GREEN_KEY = 1 << 1;

        //startRoom = new DummyRoom() { name = "start" };

        ///*
        //  _     _     _
        // [ ]-G-[ ]-R-[_]
        //  R     |
        // [S]---[_]
         
        // */
        //DummyRoom up = new DummyRoom() { name = "up" };
        //DummyRoom right = new DummyRoom() { name = "right" };
        //DummyRoom upRight = new DummyRoom() { name = "upRight" };
        //DummyRoom upRightRight = new DummyRoom() { name = "upRightRight" };

        //DummyDoor start2up = helperLinkRooms(startRoom, up);
        //DummyDoor up2upRight = helperLinkRooms(up, upRight);
        //DummyDoor start2right = helperLinkRooms(startRoom, right);
        //DummyDoor right2upRight = helperLinkRooms(right, upRight);
        //DummyDoor upRight2upRightRight = helperLinkRooms(upRight, upRightRight);

        //start2up.AccessibilityMask |= RED_KEY;
        //up2upRight.AccessibilityMask |= GREEN_KEY;
        //upRight2upRightRight.AccessibilityMask |= RED_KEY;

        //Traverse(startRoom);
	}

    private static void ClearPopulateTraversal(Room r)
    {
        if (r.mark != -1)
        {
            r.mark = -1;
            foreach (Door door in r.doors)
            {
                ClearPopulateTraversal(door.getConnectedRoom());
            }
        }
    }

    private static GameObject keyPref = null;

    public static void PopulateKeys(GameObject start)
    {
        Room r = start.GetComponent<Room>();
        int currentMask = 0;
        List<int> neededItems = null;
        List<Room> accessibleRooms = new List<Room>();
        List<Room> nowAvailableRooms = null;
        do
        {
            ClearPopulateTraversal(r);
            nowAvailableRooms = new List<Room>();
            neededItems = new List<int>();

            PopulateKeysHelper(r, currentMask, neededItems, nowAvailableRooms, 0);

            if (neededItems.Count > 0)
            {
                List<Room> newlyAvailableRooms = nowAvailableRooms.Except(accessibleRooms).ToList();
                int item = neededItems[Random.Range(0, neededItems.Count)];
                Room placeRoom = newlyAvailableRooms[Random.Range(0, newlyAvailableRooms.Count)];

                currentMask |= item;
                Debug.LogError("Placed " + item + " in " + placeRoom.name);

                var placesForKey = placeRoom.transform.Cast<Transform>().Where(c => c.gameObject.tag == "ItemDropArea").ToArray();

                if (keyPref == null)
                    keyPref = Resources.Load<GameObject>("Key");

                GameObject key = Instantiate(keyPref, placesForKey[0].transform.position + new Vector3( 0, 1, 0 ), Quaternion.identity) as GameObject;
                key.transform.SetParent(placesForKey[0].transform.parent, true);
                Destroy(placesForKey[0].gameObject);
                key.GetComponent<Key>().color = (ColorCodeValues) Mathf.RoundToInt(Mathf.Log(item,2));
            }

            accessibleRooms = nowAvailableRooms;
        }
        while (neededItems.Count > 0);
    }

    private static void PopulateKeysHelper(Room room, int currentMask, List<int> neededItems, List<Room> accessibleRooms, int currentDepth)
    {
        if (room.mark > currentDepth || room.mark == -1)
        {
            room.mark = currentDepth;
            currentDepth++;

            var arrayOfChildren = room.transform.Cast<Transform>().Where(c=>c.gameObject.tag == "ItemDropArea").ToArray();
            if( arrayOfChildren.Length > 0 )
                accessibleRooms.Add(room);

            foreach (Door door in room.doors)
            {
                Transform swdogo = (door.transform.childCount > 0 ) ? door.transform.GetChild(0) : null;
                swdogo = (swdogo == null) ? null : swdogo.GetChild(0);
                SwingDoor swdoor = (swdogo == null) ? null : swdogo.GetComponent<SwingDoor>();

                int accessibility = 0;
                if ( swdoor != null && swdoor.Locked)
                    accessibility = 1 << ((int)swdoor.color);

                int needed = (~currentMask) & accessibility;
                if (needed == 0)
                {
                    PopulateKeysHelper(door.getConnectedRoom(), currentMask, neededItems, accessibleRooms, currentDepth);
                }
                else
                {
                    neededItems.Add(needed);
                }
            }
        }
    }

    //private void ClearTraversal(DummyRoom room)
    //{
    //    if (room.mark != -1)
    //    {
    //        room.mark = -1;
    //        foreach (DummyDoor door in room.doors)
    //        {
    //            ClearTraversal(door.connectedDoor.connectedRoom);
    //        }
    //    }
    //}

    //private void Traverse(DummyRoom room)
    //{
    //    int currentMask = 0;
    //    List<int> neededItems = null;
    //    List<DummyRoom> accessibleRooms = new List<DummyRoom>();
    //    List<DummyRoom> nowAvailableRooms = null;
    //    do
    //    {
    //        ClearTraversal(room);
    //        nowAvailableRooms = new List<DummyRoom>();
    //        neededItems = new List<int>();

    //        TraverseHelper(room, currentMask, neededItems, nowAvailableRooms, 0);

    //        if (neededItems.Count > 0)
    //        {
    //            List<DummyRoom> newlyAvailableRooms = nowAvailableRooms.Except(accessibleRooms).ToList();
    //            int item = neededItems[Random.Range(0, neededItems.Count)];
    //            DummyRoom placeRoom = newlyAvailableRooms[Random.Range(0, newlyAvailableRooms.Count)];

    //            currentMask |= item;
    //            Debug.Log("Placed " + item + " in " + placeRoom.name);
    //        }

    //        accessibleRooms = nowAvailableRooms;
    //    }
    //    while ( neededItems.Count > 0);
    //}

    //private void TraverseHelper(DummyRoom room, int currentMask, List<int> neededItems, List<DummyRoom> accessibleRooms, int currentDepth )
    //{
    //    if( room.mark > currentDepth || room.mark == -1 )
    //    {
    //        room.mark = currentDepth;
    //        currentDepth++;
    //        accessibleRooms.Add(room);

    //        foreach (DummyDoor door in room.doors)
    //        {
    //            int needed = (~currentMask)&door.AccessibilityMask;
    //            if (needed == 0)
    //            {
    //                TraverseHelper(door.connectedDoor.connectedRoom, currentMask, neededItems, accessibleRooms, currentDepth );
    //            }
    //            else
    //            {
    //                neededItems.Add(needed);
    //            }
    //        }
    //    }
    //}



    //DummyDoor helperLinkRooms(DummyRoom a, DummyRoom b)
    //{
    //    DummyDoor ab = new DummyDoor();
    //    DummyDoor ba = new DummyDoor();

    //    ab.connectedDoor = ba;
    //    ba.connectedDoor = ab;
    //    ab.connectedRoom = a;
    //    ba.connectedRoom = b;

    //    a.doors.Add(ab);
    //    b.doors.Add(ba);

    //    return ab;
    //}

    //void Update () 
    //{
	
    //}
}
