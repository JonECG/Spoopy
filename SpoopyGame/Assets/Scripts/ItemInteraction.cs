using UnityEngine;
using System.Collections;

public class ItemInteraction : MonoBehaviour {

    public string info = "Just an object";
    private float grabDistance = 5;

	// Use this for initialization
	void Start ()
    {
	
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
                if (Input.GetKey(KeyCode.Space))
                {
                    transform.position = playerHead.transform.position + (playerHead.transform.forward.normalized * 2);
                    transform.rigidbody.useGravity = false;
                }
                else
                    transform.rigidbody.useGravity = true;

                if (Input.GetKey(KeyCode.F))
                {
                    if (Input.GetKey(KeyCode.Space))
                    { }
                    else
                    {
                        transform.rigidbody.AddForce(playerHead.transform.forward.normalized * 3);
                    }
                }
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
