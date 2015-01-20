using UnityEngine;
using System.Collections;

public class ItemInteraction : MonoBehaviour {

    public string info = "Just an object";
    private float grabDistance = 3;
    private bool togglePickup;
    private bool spacebarDown;
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

        spacebarDown = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        GameObject playerHead = GameObject.Find("LitCamera");

        RaycastHit hitOne;

        pickupCorrected = Debouncer.Debounce("PickUp", pickupCorrected);
        throwCorrected = Debouncer.Debounce("Throw", throwCorrected);

        if (!togglePickup && Physics.Raycast(playerHead.transform.position, transform.position - playerHead.transform.position, out hitOne) && Vector3.Angle(playerHead.transform.forward, (transform.position - playerHead.transform.position)) < 25 && (transform.position - playerHead.transform.position).magnitude < grabDistance)
        {
            HeadsUpDisplayController.Instance.DrawText(info, 0, -0.4f, Color.blue, 0.06f);
        }

        if (pickupCorrected.IsPressed())
        {
            if (Physics.Raycast(playerHead.transform.position, transform.position-playerHead.transform.position,out hitOne) && Vector3.Angle(playerHead.transform.forward, (transform.position-playerHead.transform.position))<25 && (transform.position-playerHead.transform.position).magnitude<grabDistance)
            {
                if (hitOne.collider.gameObject.tag == "Item")
                {
                    if (togglePickup && !spacebarDown)
                    {
                        togglePickup = false;
                    }
                    else if ((!togglePickup && !spacebarDown))
                    {
                        togglePickup = true;
                        Debug.Log("Pick up");
                    }
                    spacebarDown = true;
                }
            }
        }
        else
        {
            spacebarDown = false;
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
            RaycastHit info;
            Physics.Raycast(playerHead.transform.position, playerHead.transform.forward, out info, 2, 1 << LayerMask.NameToLayer("Map") );
            rigidbody.MovePosition(playerHead.transform.position + (playerHead.transform.forward.normalized * ( info.collider != null ? info.distance - 0.1f : 2 )));
            //transform.position = playerHead.transform.position + (playerHead.transform.forward.normalized * 2);
            transform.rigidbody.useGravity = false;
            //transform.rigidbody.isKinematic = true;
        }
        else
        {
            transform.rigidbody.useGravity = true;
            //transform.rigidbody.isKinematic = false;
        }
	}
}
