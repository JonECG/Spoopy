using UnityEngine;
using System.Collections;

public class FallToTop : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            GameObject transport = GameObject.Find("AtTop");

            other.transform.position = transport.transform.position;
        }
    }
}
