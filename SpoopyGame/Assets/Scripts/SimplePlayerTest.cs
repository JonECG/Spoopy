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
            rigidbody.AddForce(Vector3.forward * 20, ForceMode.Force);
        }
	}
}
