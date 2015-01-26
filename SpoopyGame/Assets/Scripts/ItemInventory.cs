using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemInventory : MonoBehaviour {

    public int numItems=0;
    static public bool opened;
    List<GameObject> objects;
    Debouncer.DebouncerResults storeCorrected;
    Debouncer.DebouncerResults openInventoryCorrected;
    Debouncer.DebouncerResults pickupCorrected;

	// Use this for initialization
	void Start ()
    {
        objects = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        storeCorrected = Debouncer.Debounce("Store", storeCorrected);
        openInventoryCorrected = Debouncer.Debounce("OpenInventory", openInventoryCorrected);
        pickupCorrected = Debouncer.Debounce("PickUp", pickupCorrected);

        ItemInteraction[] items=Object.FindObjectsOfType(typeof(ItemInteraction)) as ItemInteraction[];

        bool checkForItem=false;
        for(int i=0; i<items.Length && !checkForItem; i++)
        {
            checkForItem = items[i].togglePickup;
        }

        if (!checkForItem)
        {
            GameObject bag = GameObject.Find("ItemBag");

            GameObject playerHead = GameObject.Find("LitCamera");

            if (Vector3.Angle(playerHead.transform.forward, (transform.position - playerHead.transform.position)) < 25)
            {
                showItems();
            }

            if (opened)
            {
                for (int i = 0; i < numItems; i++)
                {
                    float angle = i / (float)numItems * 2 * Mathf.PI;
                    objects[i].transform.position = transform.position + new Vector3(Mathf.Cos(angle), 0.5f, Mathf.Sin(angle));
                }
            }
            else
            {
                for (int i = 0; i < numItems; i++)
                {
                    objects[i].transform.position = new Vector3(0.0f, -100.0f, 0.0f);
                }
            }
        }
	}

    public void showItems()
    {
        if (numItems != 0 && !ItemInteraction.lookingAtObject)
        {
            if (opened)
            {
                ItemInteraction[] items = Object.FindObjectsOfType(typeof(ItemInteraction)) as ItemInteraction[];
                GameObject playerHead = GameObject.Find("LitCamera");

                float angle=Vector3.Angle(playerHead.transform.forward, (transform.position-playerHead.transform.position));
                bool bagClosest = true;
                int itemSelect = -1;

                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i].isInBag && Vector3.Angle(playerHead.transform.forward, (items[i].transform.position - playerHead.transform.position)) < angle)
                    {
                        angle = Vector3.Angle(playerHead.transform.forward, (items[i].transform.position - playerHead.transform.position));
                        itemSelect = i;
                        bagClosest = false;
                    }
                }

                if (bagClosest)
                    HeadsUpDisplayController.Instance.DrawText("(P) to close inventory", 0, -0.4f, Color.blue, 0.06f);
                else
                {
                    HeadsUpDisplayController.Instance.DrawText(items[itemSelect].info, 0, -0.4f, Color.blue, 0.06f);
                    if (pickupCorrected.IsPressed())
                    {
                        items[itemSelect].togglePickup = true;
                        opened = false;
                    }
                }
            }
            else
                HeadsUpDisplayController.Instance.DrawText("(P) to open inventory", 0, -0.4f, Color.blue, 0.06f);

            if (openInventoryCorrected.IsPressed())
            {
                if (opened)
                    opened = false;
                else
                    opened = true;
            }
        }
    }

    public bool AskToStore()
    {
        HeadsUpDisplayController.Instance.DrawText("(T) to store item in inventory", 0, -0.4f, Color.blue, 0.06f);

        if (storeCorrected.IsPressed())
        {
            return true;
        }
        return false;
    }

    public void removeItem(GameObject item)
    {
        if (objects.Contains(item))
        {
            objects.Remove(item);
            numItems--;
        }
    }

    public void StoreItem(GameObject item)
    {
        objects.Add(item);
        numItems++;
    }
}
