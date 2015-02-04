using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemHandler : MonoBehaviour
{
    Debouncer.DebouncerResults pickupCorrected;
    Debouncer.DebouncerResults openInventory;
    Debouncer.DebouncerResults storeCorrected;
    Debouncer.DebouncerResults interactCorrected;

    ItemInventory inventory;
    //ItemInteraction[] items;
    //DoorKnob[] doors;

	// Use this for initialization
	void Start ()
    {
        inventory = FindObjectOfType<ItemInventory>();
        //items = FindObjectsOfType<ItemInteraction>();
        //doors = FindObjectsOfType<DoorKnob>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        pickupCorrected = Debouncer.Debounce("PickUp", pickupCorrected);
        openInventory = Debouncer.Debounce("OpenInventory", openInventory);
        storeCorrected = Debouncer.Debounce("Store", storeCorrected);
        interactCorrected = Debouncer.Debounce("Interact", interactCorrected);

        List<ItemInteraction> items = ItemInteraction.allItems;
        List<DoorKnob> doors = DoorKnob.allDoors;

        float angle = 360.0f;

        GameObject playerHead = GameObject.Find("LitCamera");

        angle = Vector3.Angle(playerHead.transform.forward, inventory.transform.position - playerHead.transform.position);

        int selection = -1;
        for (int i = 0; i < items.Count && angle>0; i++)
        {
            if (items[i] != null)
            {
                if (Vector3.Distance(playerHead.transform.position, items[i].transform.position) < 3 && Vector3.Angle(playerHead.transform.forward, items[i].transform.position - playerHead.transform.position) < angle)
                {
                    angle = Vector3.Angle(playerHead.transform.forward, items[i].transform.position - playerHead.transform.position);
                    selection = i;
                }

                if (items[i].isPickedUp)
                {
                    angle = 0;
                    selection = i;
                }
            }
        }

        for (int i = 0; i < doors.Count && angle > 0; i++)
        {
            if (doors[i] != null)
            {
                if (Vector3.Distance(playerHead.transform.position, doors[i].transform.position) < 3 && Vector3.Angle(playerHead.transform.forward, doors[i].transform.position - playerHead.transform.position) < angle)
                {
                    angle = Vector3.Angle(playerHead.transform.forward, doors[i].transform.position - playerHead.transform.position);
                    selection = i + items.Count;
                }
                if (doors[i].isGrabbed)
                {
                    angle = 0;
                    selection = i + items.Count;
                }
            }
        }

        if (angle < 25)
        {
            if (selection >= 0 && selection < items.Count)
            {
                //Picking up
                if (pickupCorrected.IsPressed() && !items[selection].isPickedUp)
                {
                    TakeItemTriggerer t = items[selection].GetComponent<TakeItemTriggerer>();
                    if( t != null )
                        t.Take();

                    if (items[selection].isTakeable)
                    {
                        Destroy(items[selection].gameObject);
                    }
                    else
                    {
                        items[selection].isPickedUp = true;
                        if (ItemInventory.isInBag(items[selection]))
                        {
                            ItemInventory.removeItem(items[selection]);
                            ItemInventory.isOpen = false;
                        }
                        if (ItemInventory.isOpen)
                            ItemInventory.isOpen = false;
                    }
                }
                //Dropping/storing
                else if (items[selection].isPickedUp && Vector3.Distance(items[selection].transform.position, inventory.transform.position) < 0.5)
                {
                    if (pickupCorrected.IsPressed())
                    {
                        items[selection].isPickedUp = false;
                    }

                    HeadsUpDisplayController.Instance.DrawText("Store Item", 0, 0, Color.blue);
                    if (storeCorrected.IsPressed())
                    {
                        items[selection].isPickedUp = false;
                        ItemInventory.storeItem(items[selection]);
                    }
                }
                //Just dropping
                else if (pickupCorrected.IsPressed() && items[selection].isPickedUp)
                {
                    items[selection].isPickedUp = false;
                }
                //Prompt for picking up
                else if (!items[selection].isPickedUp)
                {
                    HeadsUpDisplayController.Instance.DrawText(items[selection].info, 0, 0, Color.blue);
                    HeadsUpDisplayController.Instance.DrawText("Press (A) to " + (items[selection].isTakeable ? "Take" : "Pick up"), 0, 0.2f, Color.blue);
                }
            }
            else if (selection >= (items.Count))
            {
                if (interactCorrected.IsPressed() && !doors[selection - items.Count].isGrabbed)
                {
                    doors[selection - items.Count].isGrabbed = true;
                    doors[selection - items.Count].door.RequestUnlatch();
                    doors[selection - items.Count].grabbedDistance = Vector3.Distance(doors[selection - items.Count].transform.position, playerHead.transform.position);
                }
                else if ((interactCorrected.IsReleased() || (Vector3.Distance(playerHead.transform.position, doors[selection - items.Count].transform.position) > 3)) && doors[selection - items.Count].isGrabbed)
                {
                    doors[selection - items.Count].isGrabbed = false;
                    doors[selection - items.Count].door.RequestLatch();
                }

                if (doors[selection - items.Count].door.Locked)
                    HeadsUpDisplayController.Instance.DrawText("Locked -- Needs " + doors[selection - items.Count].door.adjColor.Name.ToUpper() + " Key", 0, 0, doors[selection - items.Count].door.adjColor.Color);
                else if (doors[selection - items.Count].isGrabbed)
                    HeadsUpDisplayController.Instance.DrawText("Turn to Move the Door",0,0,Color.blue);
                else if (!doors[selection - items.Count].isGrabbed)
                    HeadsUpDisplayController.Instance.DrawText("Hold (A) to Grab Handle",0,0,Color.blue);
                //Add more else ifs for more heads up displays if needed, I'm going to bed
            }
            else
            {
                //Inventory
                if (openInventory.IsPressed() && !ItemInventory.isOpen)
                {
                    ItemInventory.isOpen = true;
                }
                else if (openInventory.IsPressed() && ItemInventory.isOpen)
                {
                    ItemInventory.isOpen = false;
                }
                else if (!ItemInventory.isOpen && ItemInventory.objects.Count > 0)
                    HeadsUpDisplayController.Instance.DrawText("Press (A) to Open Inventory", 0, 0, Color.blue);
                else if (ItemInventory.isOpen)
                    HeadsUpDisplayController.Instance.DrawText("Press (A) to Close Inventory", 0, 0, Color.blue);
            }
        }
	}
}
