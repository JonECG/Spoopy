using UnityEngine;
using System.Collections;

public class DestroyOnTrigger : MonoBehaviour {

    private Vector3 origPosition;

    void Start()
    {
        origPosition = transform.position;
    }

	void OnTriggerEnter()
    {
        transform.position = origPosition;
    }
}
