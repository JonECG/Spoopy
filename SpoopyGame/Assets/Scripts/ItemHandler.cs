using UnityEngine;
using System.Collections;

public class ItemHandler : MonoBehaviour
{
    Debouncer.DebouncerResults pickupCorrected;
    Debouncer.DebouncerResults openInventory;
    Debouncer.DebouncerResults storeCorrected;
    Debouncer.DebouncerResults interactCorrected;

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
        pickupCorrected = Debouncer.Debounce("PickUp", pickupCorrected);
        openInventory = Debouncer.Debounce("OpenInventory", openInventory);
        storeCorrected = Debouncer.Debounce("Store", storeCorrected);
        interactCorrected = Debouncer.Debounce("Interact", interactCorrected);

        ItemInventory inventory = FindObjectOfType<ItemInventory>();

        ItemInteraction[] items = FindObjectsOfType<ItemInteraction>();

        DoorKnob[] doors = FindObjectsOfType<DoorKnob>();

        float angle = 360.0f;

        GameObject playerHead = GameObject.Find("LitCamera");

        angle = Vector3.Angle(playerHead.transform.forward, transform.position - playerHead.transform.position);

        int selection = -1;
        for (int i = 0; i < items.Length && angle>0; i++)
        {
            if (Vector3.Distance(playerHead.transform.position, items[i].transform.position)<3 && Vector3.Angle(playerHead.transform.forward, items[i].transform.position - playerHead.transform.position) < angle)
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

        for (int i = 0; i < doors.Length && angle > 0; i++)
        {
            if (Vector3.Distance(playerHead.transform.position, doors[i].transform.position) < 3 && Vector3.Angle(playerHead.transform.forward, doors[i].transform.position - playerHead.transform.position) < angle)
            {
                angle = Vector3.Angle(playerHead.transform.forward, doors[i].transform.position - playerHead.transform.position);
                selection = i+items.Length;
            }
            if (doors[i].isGrabbed)
            {
                angle = 0;
                selection = i+items.Length;
            }
        }

        if (angle < 25)
        {
            if (selection >=0  && selection<items.Length)
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
            else if (selection >= (items.Length))
            {
                if (interactCorrected.IsPressed() && !doors[selection - items.Length].isGrabbed)
                {
                    doors[selection - items.Length].isGrabbed = true;
                    doors[selection - items.Length].door.RequestUnlatch();
                    doors[selection - items.Length].grabbedDistance = Vector3.Distance(doors[selection - items.Length].transform.position, playerHead.transform.position);
                }
                else if ((interactCorrected.IsReleased() || (Vector3.Distance(playerHead.transform.position, doors[selection - items.Length].transform.position) > 3)) && doors[selection - items.Length].isGrabbed)
                {
                    doors[selection - items.Length].isGrabbed = false;
                    doors[selection - items.Length].door.RequestLatch();
                }

                if (doors[selection - items.Length].door.Locked)
                    HeadsUpDisplayController.Instance.DrawText("Locked -- Needs " + doors[selection - items.Length].door.adjColor.Name.ToUpper() + " Key", 0, 0, doors[selection - items.Length].door.adjColor.Color );
                else if (doors[selection - items.Length].isGrabbed)
                    HeadsUpDisplayController.Instance.DrawText("Turn to Move the Door",0,0,Color.blue);
                else if (!doors[selection - items.Length].isGrabbed)
                    HeadsUpDisplayController.Instance.DrawText("Press (A) to Grab Handle",0,0,Color.blue);
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
