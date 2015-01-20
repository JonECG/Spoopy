using UnityEngine;
using System.Collections;

public class RoomGeneratorScript : MonoBehaviour 
{

    public GameObject Floor;
    public GameObject Corner;
    public GameObject Wall;
    public GameObject DoorWay;
    public GameObject HallWay;
    public enum Directions{UP,DOWN,LEFT,RIGHT};
    private int tileWidth = 2;
    private int tileHeight = 2;
	// Use this for initialization
	void Start () 
    {
        //getRectHallWay(7, 7, true, true, false, false);

        //GameObject temp = getRectRoom(7, 7, true, false, false, false);
        //temp.transform.Translate(new Vector3(0, 0, tileWidth * 7));
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public GameObject getRectHallWay(int XTileWidth, int YTileHeight, bool northDoor, bool eastDoor, bool southDoor, bool westDoor)
    {
        GameObject parentRoomObject = new GameObject();


        for (int i = 0; i < XTileWidth; i++)
        {
            for (int j = 0; j < YTileHeight; j++)
            {
                if (i < Mathf.Round(XTileWidth / 2) && j == Mathf.Round(YTileHeight / 2) && eastDoor)
                {
                    GameObject temp2 = (GameObject)Instantiate(HallWay);
                    temp2.transform.parent = parentRoomObject.transform;
                    temp2.transform.Translate(new Vector3(i * tileWidth - (XTileWidth / 2), 0, j * tileHeight - (YTileHeight / 2)));
                }
                else
                if (i > Mathf.Round(XTileWidth / 2) && j == Mathf.Round(YTileHeight / 2) && westDoor)
                {
                    GameObject temp2 = (GameObject)Instantiate(HallWay);
                    temp2.transform.parent = parentRoomObject.transform;
                    temp2.transform.Translate(new Vector3(i * tileWidth - (XTileWidth / 2), 0, j * tileHeight - (YTileHeight / 2)));
                }
                else
                if (j < Mathf.Round(YTileHeight / 2) && i == Mathf.Round(XTileWidth / 2) && northDoor)
                {
                    GameObject temp2 = (GameObject)Instantiate(HallWay);
                    temp2.transform.parent = parentRoomObject.transform;
                    temp2.transform.Translate(new Vector3(i * tileWidth - (XTileWidth / 2), 0, j * tileHeight - (YTileHeight / 2)));
                    temp2.transform.Rotate(new Vector3(0, 90, 0));
                }
                else
                if (j > Mathf.Round(YTileHeight / 2) && i == Mathf.Round(XTileWidth / 2) && southDoor)
                {
                    GameObject temp2 = (GameObject)Instantiate(HallWay);
                    temp2.transform.parent = parentRoomObject.transform;
                    temp2.transform.Translate(new Vector3(i * tileWidth - (XTileWidth / 2),0, j * tileHeight - (YTileHeight / 2)));
                    temp2.transform.Rotate(new Vector3(0, 90, 0));
                }
                else
                if (i == Mathf.Round(XTileWidth/2) && j == Mathf.Round(YTileHeight/2))
                {
                    if (northDoor && southDoor && eastDoor && westDoor)
                    {
                        GameObject temp2 = (GameObject)Instantiate(Floor);
                        temp2.transform.parent = parentRoomObject.transform;
                        temp2.transform.Translate(new Vector3(i * tileWidth - (XTileWidth / 2), 0, j * tileHeight - (YTileHeight / 2)));
                    }
                    else
                    if (northDoor && southDoor && eastDoor)
                    {
                        GameObject temp2 = (GameObject)Instantiate(Wall);
                        temp2.transform.parent = parentRoomObject.transform;
                        temp2.transform.Translate(new Vector3(i * tileWidth - (XTileWidth / 2), 0, j * tileHeight - (YTileHeight / 2)));
                        temp2.transform.Rotate(new Vector3(0, 90, 0));
                    }
                    else
                    if (northDoor && southDoor && westDoor)
                    {
                        GameObject temp2 = (GameObject)Instantiate(Wall);
                        temp2.transform.parent = parentRoomObject.transform;
                        temp2.transform.Translate(new Vector3(i * tileWidth - (XTileWidth / 2), 0, j * tileHeight - (YTileHeight / 2)));
                        temp2.transform.Rotate(new Vector3(0, -90, 0));
                    }
                    else
                    if (northDoor && westDoor && eastDoor)
                    {
                        GameObject temp2 = (GameObject)Instantiate(Wall);
                        temp2.transform.parent = parentRoomObject.transform;
                        temp2.transform.Translate(new Vector3(i * tileWidth - (XTileWidth / 2), 0, j * tileHeight - (YTileHeight / 2)));
                    }
                    else
                    if (southDoor && westDoor && eastDoor)
                    {
                        GameObject temp2 = (GameObject)Instantiate(Wall);
                        temp2.transform.parent = parentRoomObject.transform;
                        temp2.transform.Translate(new Vector3(i * tileWidth - (XTileWidth / 2), 0, j * tileHeight - (YTileHeight / 2)));
                        temp2.transform.Rotate(new Vector3(0, 180, 0));
                    }
                    else
                    if (southDoor && northDoor)
                    {
                        GameObject temp2 = (GameObject)Instantiate(HallWay);
                        temp2.transform.parent = parentRoomObject.transform;
                        temp2.transform.Translate(new Vector3(i * tileWidth - (XTileWidth / 2), 0, j * tileHeight - (YTileHeight / 2)));
                        //temp2.transform.Rotate(new Vector3(0, 180, 0));
                        temp2.transform.Rotate(new Vector3(0, 90, 0));
                    }
                    else
                    if (southDoor && westDoor)
                    {
                        GameObject temp2 = (GameObject)Instantiate(Corner);
                        temp2.transform.parent = parentRoomObject.transform;
                        temp2.transform.Translate(new Vector3(i * tileWidth - (XTileWidth / 2), 0, j * tileHeight - (YTileHeight / 2)));
                        //temp2.transform.Rotate(new Vector3(0, 180, 0));
                        Rotate(4, 0, 4, 0, temp2);
                    }
                    else
                    if (southDoor && eastDoor)
                    {
                        GameObject temp2 = (GameObject)Instantiate(Corner);
                        temp2.transform.parent = parentRoomObject.transform;
                        temp2.transform.Translate(new Vector3(i * tileWidth - (XTileWidth / 2), 0, j * tileHeight - (YTileHeight / 2)));
                        //temp2.transform.Rotate(new Vector3(0, 180, 0));
                        Rotate(0, 0, 4, 0, temp2);
                    }
                    else
                    if (northDoor && westDoor)
                    {
                        GameObject temp2 = (GameObject)Instantiate(Corner);
                        temp2.transform.parent = parentRoomObject.transform;
                        temp2.transform.Translate(new Vector3(i * tileWidth - (XTileWidth / 2), 0, j * tileHeight - (YTileHeight / 2)));
                        //temp2.transform.Rotate(new Vector3(0, 180, 0));
                        Rotate(0, 4, 4, 4, temp2);
                    }
                    else
                    if (northDoor && eastDoor)
                    {
                        GameObject temp2 = (GameObject)Instantiate(Corner);
                        temp2.transform.parent = parentRoomObject.transform;
                        temp2.transform.Translate(new Vector3(i * tileWidth - (XTileWidth / 2), 0, j * tileHeight - (YTileHeight / 2)));
                        //temp2.transform.Rotate(new Vector3(0, 180, 0));
                        Rotate(4, 4, 4, 4, temp2);
                    }
                    else
                    if (westDoor && eastDoor)
                    {
                        GameObject temp2 = (GameObject)Instantiate(HallWay);
                        temp2.transform.parent = parentRoomObject.transform;
                        temp2.transform.Translate(new Vector3(i * tileWidth - (XTileWidth / 2), 0, j * tileHeight - (YTileHeight / 2)));
                    }
                }


                    
            }
        }


        return parentRoomObject;
    }

    public GameObject getRectRoom(int XTileWidth, int YTileHeight,bool northDoor, bool eastDoor, bool southDoor, bool westDoor)
    {
        GameObject parentRoomObject = new GameObject();

        for (int i = 0; i < XTileWidth; i++)
        {
            for (int j = 0; j < YTileHeight; j++)
            {
                //Check for doors first
                if ((i == 0 && j == 0) || (i == 0 && j == (YTileHeight - 1)) || (i == (XTileWidth - 1) && j == 0) || (i == (XTileWidth - 1) && j == (YTileHeight - 1)))
                { //Corners
                    GameObject temp = (GameObject)Instantiate(Corner);
                    temp.transform.parent = parentRoomObject.transform;
                    temp.transform.Translate(new Vector3(i * tileWidth - (XTileWidth / 2),0, j * tileHeight - (YTileHeight / 2)));
                    Rotate(i, j, XTileWidth, YTileHeight, temp);
                }
                else
                if ((i == 0) || (j == 0) || i == (XTileWidth-1) || j == (YTileHeight-1))
                { //Walls and Doors
                    bool isDoor = false;
                    GameObject temp = null;

                    if (i == 0 && j == Mathf.Round(YTileHeight / 2) && westDoor)
                    {
                        temp = (GameObject)Instantiate(DoorWay);
                        isDoor = true;
                    }
                    else
                    if (i == Mathf.Round(XTileWidth / 2) && j == 0 && northDoor)
                    {
                        temp = (GameObject)Instantiate(DoorWay);
                        isDoor = true;
                    }
                    else
                    if (i == Mathf.Round(XTileWidth / 2) && j == YTileHeight-1 && southDoor)
                    {
                        temp = (GameObject)Instantiate(DoorWay);
                        isDoor = true;
                    }
                    else
                    if (i == XTileWidth - 1 && j == Mathf.Round(YTileHeight / 2) && eastDoor)
                    {
                        temp = (GameObject)Instantiate(DoorWay);
                        isDoor = true;
                    }
                    else
                    {
                        temp = (GameObject)Instantiate(Wall);
                    }
                    temp.transform.parent = parentRoomObject.transform;
                    temp.transform.Translate(new Vector3(i * tileWidth - (XTileWidth / 2),0, j * tileHeight - (YTileHeight / 2)));
                    Rotate(i, j, XTileWidth, YTileHeight, temp);
                    if (isDoor)
                    {
                        temp.transform.Rotate(new Vector3(0, -90, 0));
                    }
                }
                else
                { //Floor
                    GameObject temp = (GameObject)Instantiate(Floor);
                    temp.transform.parent = parentRoomObject.transform;
                    temp.transform.Translate(new Vector3(i * tileWidth - (XTileWidth / 2),0, j * tileHeight - (YTileHeight / 2)));
                    //Rotate(i, j, XTileWidth, YTileHeight, temp);
                }
            }
        }

        return parentRoomObject;
    }

    private void Rotate(int i, int j, int maxI, int maxJ, GameObject toRotate)
    {
        if (i == 0 && j == 0)
        {
            toRotate.transform.Rotate(new Vector3(0, 180, 0));
        }
        else
        if (i == maxI - 1 && j == 0)
        {
            toRotate.transform.Rotate(new Vector3(0, 90, 0));
        }
        else
        if (i == 0 && j == maxJ-1)
        {
            toRotate.transform.Rotate(new Vector3(0, 270, 0));
        }
        else
        if (i == maxI - 1 && j == maxJ-1)
        {
            //Doesnt need rotation
        }
        else
        if (i == 0)
        {
            toRotate.transform.Rotate(new Vector3(0, 270, 0));
        }
        else
        if (i == maxI-1)
        {
            toRotate.transform.Rotate(new Vector3(0, 90, 0));
        }
        else
        if (j == 0)
        {
            toRotate.transform.Rotate(new Vector3(0, 180, 0));
        }
        else
        if (j == maxJ-1)
        {
            //Doesn't need rotation
        }
        else
        {
            toRotate.transform.Rotate(new Vector3(0, Random.Range(0, 3) * 90, 0));
        }
    }

    public Directions randomDirection()
    {
        int test = Random.Range(0, 3);
        Directions ret = Directions.UP;
        switch (test)
        {
            case 0:
                ret = Directions.UP;
            break;
            case 1:
                ret = Directions.RIGHT;
            break;
            case 2:
                ret = Directions.DOWN;
            break;
            case 3:
                ret = Directions.LEFT;
            break;
        }
        return ret;
    }
}
