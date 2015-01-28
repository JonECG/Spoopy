using UnityEngine;
using System.Collections;

public class ItemHandler : MonoBehaviour
{
    Debouncer.DebouncerResults pickupCorrected;
    Debouncer.DebouncerResults openInventory;
    Debouncer.DebouncerResults storeCorrected;

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

        ItemInventory inventory = FindObjectOfType<ItemInventory>();

        ItemInteraction[] items = FindObjectsOfType<ItemInteraction>();

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

        if (angle < 25)
        {
            if (selection >= 0)
            {
                if (pickupCorrected.IsPressed() && !items[selection].isPickedUp)
                {
                    items[selection].isPickedUp = true;
                    if(ItemInventory.isInBag(items[selection]))
                    {
                        ItemInventory.removeItem(items[selection]);
                        ItemInventory.isOpen = false;
                    }
                    if (ItemInventory.isOpen)
                        ItemInventory.isOpen = false;
                }
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
                else if (pickupCorrected.IsPressed() && items[selection].isPickedUp)
                {
                    items[selection].isPickedUp = false;
                }
                else if (!items[selection].isPickedUp)
                    HeadsUpDisplayController.Instance.DrawText(items[selection].info, 0, 0, Color.blue);
            }
            else
            {
                if (openInventory.IsPressed() && !ItemInventory.isOpen)
                {
                    ItemInventory.isOpen = true;
                }
                else if (openInventory.IsPressed() && ItemInventory.isOpen)
                {
                    ItemInventory.isOpen = false;
                }
                else if (!ItemInventory.isOpen && ItemInventory.objects.Count>0)
                    HeadsUpDisplayController.Instance.DrawText("Open Inventory", 0, 0, Color.blue);
                else if (ItemInventory.isOpen)
                    HeadsUpDisplayController.Instance.DrawText("Close Inventory", 0, 0, Color.blue);
            }
        }
	}
}
