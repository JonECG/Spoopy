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

        if (Application.isEditor)
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

        transform.position += new Vector3(forwardReference.x, 0, forwardReference.z).normalized * Time.deltaTime * moveSpeed * Input.GetAxis("Vertical");
        transform.position += new Vector3(rightReference.x, 0, rightReference.z).normalized * Time.deltaTime * moveSpeed * Input.GetAxis("Horizontal");
        //if (Input.GetAxis("Vertical") > 0.0f)
        //{
        //    transform.position += new Vector3(forwardReference.x, 0, forwardReference.z).normalized * Time.deltaTime * moveSpeed;
        //}
        //if (Input.GetAxis("Vertical") < 0.0f)
        //{
        //    transform.position -= new Vector3(forwardReference.x, 0, forwardReference.z).normalized * Time.deltaTime * moveSpeed;
        //}
        //if (Input.GetAxis("Horizontal") > 0.0f)
        //{
        //    transform.position += new Vector3(rightReference.x, 0, rightReference.z).normalized * Time.deltaTime * moveSpeed;
        //}
        //if (Input.GetAxis("Horizontal") < 0.0f)
        //{
        //    transform.position -= new Vector3(rightReference.x, 0, rightReference.z).normalized * Time.deltaTime * moveSpeed;
        //}

        float lateral = Input.GetAxis("Mouse X");
        float longinal = Input.GetAxis("Mouse Y");
        float longinalStick = Input.GetAxis("TurningX");

        transform.Rotate(new Vector3(0, 1, 0), (lateral + longinalStick) * mouseSensitivity );
        head.Rotate(new Vector3(1, 0, 0), -(longinal) * mouseSensitivity);
	
	}
}
