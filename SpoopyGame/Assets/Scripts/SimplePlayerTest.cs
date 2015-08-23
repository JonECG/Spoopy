using UnityEngine;
using System.Collections;

public class SimplePlayerTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.W) == true)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.forward * 20, ForceMode.Force);
        }
	}
}
