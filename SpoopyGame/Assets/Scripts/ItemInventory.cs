using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemInventory : MonoBehaviour
{
    static public bool isOpen;
    static public List<GameObject> objects;

	// Use this for initialization
	void Start ()
    {
        isOpen = false;
        objects = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (isOpen)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                float angle = i / (float)objects.Count * 2 * Mathf.PI;
                objects[i].transform.position = transform.position + new Vector3(Mathf.Cos(angle), 0.5f, Mathf.Sin(angle));
            }
        }
	}

    static public void storeItem(ItemInteraction item)
    {
        objects.Add(item.gameObject);
    }
    static public void removeItem(ItemInteraction item)
    {
        objects.Remove(item.gameObject);
    }

    static public bool isInBag(ItemInteraction item)
    {
        bool found=false;
        for (int i = 0; i < objects.Count && !found; i++)
        {
            found = (objects[i] == item.gameObject);
        }
        return found;
    }
}
