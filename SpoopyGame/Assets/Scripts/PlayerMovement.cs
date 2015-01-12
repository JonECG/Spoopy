using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {


    Transform anchor;
    Transform head;
    int moveSpeed = 3;
    float mouseSensitivity = 2.0f;

	// Use this for initialization
	void Start () {
        head = GameObject.Find("Head").transform;

        if (1 == 2)//!Application.isEditor)
        {
            GameObject.Destroy(GameObject.Find("OVRCameraRig"));
            anchor = GameObject.Find("Head").transform;
        }
        else
        {
            GameObject.Destroy(GameObject.Find("NormalCamera"));
            anchor = GameObject.Find("CenterEyeAnchor").transform;
        }

        GameObject.Find("LitCamera").transform.parent = anchor;
        GameObject.Find("Headlamp").transform.parent = anchor;
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 rightReference = anchor.rotation * new Vector3(1, 0, 0);
        Vector3 upReference = anchor.rotation * new Vector3(0, 1, 0);
        Vector3 forwardReference = anchor.rotation * new Vector3(0, 0, 1);

        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(forwardReference.x, 0, forwardReference.z).normalized * Time.deltaTime * moveSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= new Vector3(forwardReference.x, 0, forwardReference.z).normalized * Time.deltaTime * moveSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(rightReference.x, 0, rightReference.z).normalized * Time.deltaTime * moveSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= new Vector3(rightReference.x, 0, rightReference.z).normalized * Time.deltaTime * moveSpeed;
        }

        float lateral = Input.GetAxis("Mouse X");
        float longinal = Input.GetAxis("Mouse Y");

        transform.Rotate(new Vector3(0, 1, 0), lateral * mouseSensitivity );
        head.Rotate(new Vector3(1, 0, 0), -longinal * mouseSensitivity);
	
	}
}
