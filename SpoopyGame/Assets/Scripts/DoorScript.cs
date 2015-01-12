using UnityEngine;
using System.Collections;

public class DoorScript : MonoBehaviour {

    public GameObject TargetKey;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.parent == TargetKey)
        {
            Destroy(this);
        }
    }
}
