using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemInteraction : MonoBehaviour {

    public string info = "Just an object";
    public bool isPickedUp;
    public bool isTakeable = false;
    Debouncer.DebouncerResults throwCorrected;

    float lastTimeNotHeld;

    public static List<ItemInteraction> allItems = new List<ItemInteraction>();

	// Use this for initialization
	void Start ()
    {
        allItems.Add(this);
        Renderer[] rends = GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in rends)
        {
            //Creates a new array of materials, one larger to fit new material
            var mats = renderer.materials;
            Material[] newMats = new Material[mats.Length + 1];
            for (int i = 0; i < mats.Length; i++)
            {
                newMats[i] = mats[i];
            }
            newMats[mats.Length] = Resources.Load<Material>("InteractableOverlay");
            renderer.materials = newMats;
        }
	}

    void OnDestroy()
    {
        allItems.Remove(this);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isPickedUp && collision.gameObject.layer == LayerMask.NameToLayer("Map") )
        {
            transform.parent = collision.transform.root;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        throwCorrected = Debouncer.Debounce("Throw", throwCorrected);
        if (isPickedUp)
        {
            transform.parent = null;
            GameObject playerHead = GameObject.Find("LitCamera");

            RaycastHit info;
            Physics.Raycast(playerHead.transform.position, playerHead.transform.forward, out info, 2, 1 << LayerMask.NameToLayer("Map"));
            transform.position = (playerHead.transform.position + (playerHead.transform.forward.normalized * (info.collider != null ? info.distance - 0.1f : 2)));

            rigidbody.velocity = new Vector3();

            if (throwCorrected.IsPressed())
            {
                isPickedUp = false;
            }

            if (Time.time - lastTimeNotHeld > 20 && ItemInventory.objects.Count == 0)
            {
                HeadsUpDisplayController.Instance.DrawText("You can store multiple items in your inventory", 0, 0.5f, Color.yellow, 0.05f);
                HeadsUpDisplayController.Instance.DrawText("To access your inventory look down to your left", 0, 0.3f, Color.yellow, 0.05f);
            }
        }
        else
        {
            lastTimeNotHeld = Time.time;
        }

        if (ItemInventory.isInBag(this) && !ItemInventory.isOpen)
        {
            transform.position = (new Vector3(0, -100.0f, 0));
        }
	}
}
