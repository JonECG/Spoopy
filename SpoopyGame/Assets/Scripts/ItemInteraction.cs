﻿using UnityEngine;
using System.Collections;

public class ItemInteraction : MonoBehaviour {

    public string info = "Just an object";
    private float grabDistance = 3;
    public bool togglePickup;
    static public bool lookingAtObject;
    public bool isInBag;
    Debouncer.DebouncerResults pickupCorrected;
    Debouncer.DebouncerResults throwCorrected;

	// Use this for initialization
	void Start ()
    {
        //Creates a new array of materials, one larger to fit new material
        var mats = renderer.materials;
        Material[] newMats = new Material[mats.Length+1];
        for (int i = 0; i < mats.Length; i++)
        {
            newMats[i] = mats[i];
        }
        newMats[mats.Length] = Resources.Load<Material>("InteractableOverlay");
        renderer.materials = newMats;
	}
	
	// Update is called once per frame
	void Update ()
    {
        GameObject playerHead = GameObject.Find("LitCamera");

        pickupCorrected = Debouncer.Debounce("PickUp", pickupCorrected);
        throwCorrected = Debouncer.Debounce("Throw", throwCorrected);

        RaycastHit hitOne;

        if (!ItemInventory.opened && !togglePickup && Physics.Raycast(playerHead.transform.position, transform.position - playerHead.transform.position, out hitOne) && Vector3.Angle(playerHead.transform.forward, (transform.position - playerHead.transform.position)) < 25 && (transform.position - playerHead.transform.position).magnitude < grabDistance)
        {
            HeadsUpDisplayController.Instance.DrawText(info, 0, -0.4f, Color.blue, 0.06f);
            lookingAtObject = true;
        }
        else
            lookingAtObject = false;

        if (pickupCorrected.IsPressed() && !ItemInventory.opened)
        {
            if (Physics.Raycast(playerHead.transform.position, transform.position-playerHead.transform.position,out hitOne) && Vector3.Angle(playerHead.transform.forward, (transform.position-playerHead.transform.position))<25 && (transform.position-playerHead.transform.position).magnitude<grabDistance)
            {
                if (hitOne.collider.gameObject.tag == "Item")
                {
                    if (togglePickup)
                    {
                        togglePickup = false;
                    }
                    else if ((!togglePickup))
                    {
                        togglePickup = true;
                        Debug.Log("Pick up");
                    }
                }
            }
        }

        if (throwCorrected.IsPressed() && togglePickup)
        {
            if (Physics.Raycast(playerHead.transform.position, playerHead.transform.forward.normalized, out hitOne) && (transform.position - playerHead.transform.position).magnitude < grabDistance)
            {
                if (hitOne.collider.gameObject.tag == "Item")
                {
                    transform.rigidbody.velocity=((playerHead.transform.forward * 1000)*Time.deltaTime);
                    togglePickup = false;
                }
            }
        }

        if (togglePickup)
        {
            ItemInventory.opened = false;
            RaycastHit info;
            Physics.Raycast(playerHead.transform.position, playerHead.transform.forward, out info, 2, 1 << LayerMask.NameToLayer("Map") );
            transform.position= (playerHead.transform.position + (playerHead.transform.forward.normalized * (info.collider != null ? info.distance - 0.1f : 2)));
            transform.rigidbody.useGravity = false;

            GameObject bag = GameObject.Find("ItemBag");

            ItemInventory item = (ItemInventory)bag.GetComponent("ItemInventory");

            item.removeItem(gameObject);
            isInBag = false;

            if (Vector3.Distance(bag.transform.position, transform.position) < 0.5)
            {
                if (item.AskToStore())
                {
                    item.StoreItem(gameObject);
                    isInBag = true;
                    togglePickup = false;
                }
            }
        }
        else
        {
            transform.rigidbody.useGravity = true;
            //transform.rigidbody.isKinematic = false;
        }
	}
}
