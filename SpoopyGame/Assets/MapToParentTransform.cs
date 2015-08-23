using UnityEngine;
using System.Collections;

public class MapToParentTransform : MonoBehaviour {

    private Vector3 lastPosition;
    private Quaternion lastRotation;

	void Start () 
	{
        lastPosition = transform.position;
        lastRotation = transform.rotation;
        if (transform.parent.GetComponent<Rigidbody>() == null)
            transform.parent.gameObject.AddComponent<Rigidbody>();
	}
	
	void Update () 
	{
        //transform.parent.position += transform.position - lastPosition;
        //transform.parent.rotation = transform.rotation;

        transform.parent.GetComponent<Rigidbody>().MovePosition(transform.position);// - lastPosition);
        transform.parent.GetComponent<Rigidbody>().MoveRotation(transform.rotation);//Quaternion.Inverse(lastRotation) * transform.rotation);

        transform.position = transform.parent.position;
        transform.rotation = transform.parent.rotation;
        lastPosition = transform.position;
        lastRotation = transform.rotation;
	}
}
