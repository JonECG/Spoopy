using UnityEngine;
using System.Collections;

public class ItemInteraction : MonoBehaviour {

    public string info = "Just an object";
    private float grabDistance = 5;
    private bool togglePickup;
    private bool spacebarDown;

	// Use this for initialization
	void Start ()
    {
        spacebarDown = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        GameObject playerHead = GameObject.Find("Head");

        RaycastHit hitOne;

        if (Physics.Raycast(playerHead.transform.position, playerHead.transform.forward.normalized, out hitOne) && (transform.position-playerHead.transform.position).magnitude<grabDistance)
        {
            if (hitOne.collider.gameObject.tag == "Item")
            {
                Debug.Log("Raycast hit");
                if (Input.GetAxis("PickUp") > 0.0f)
                {
                    Debug.Log("Pick up pressed");
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
                else
                {
                    spacebarDown = false;
                }

                if (Input.GetKey(KeyCode.F) && togglePickup)
                {
                    transform.rigidbody.velocity=((playerHead.transform.forward * 1000)*Time.deltaTime);
                    togglePickup = false;
                }
            }

            if (togglePickup)
            {
                transform.position = playerHead.transform.position + (playerHead.transform.forward.normalized * 2);
                transform.rigidbody.useGravity = false;
            }
            else
            {
                transform.rigidbody.useGravity = true;
            }
        }
	}

    void OnGUI()
    {
        if (Input.GetKey(KeyCode.R))
        {
            GUIStyle GUIS = new GUIStyle();
            GUIS.alignment = TextAnchor.MiddleCenter;
            GUIS.normal.textColor = Color.white;
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 0, 0), info, GUIS);
        }
    }
}
