using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoorKnob : MonoBehaviour {

    public SwingDoor door;
    GameObject looking;

    public bool isGrabbed = false;
    public float grabbedDistance;

    public static List<DoorKnob> allDoors = new List<DoorKnob>();

	void Start () 
	{
        door = transform.parent.GetComponent<SwingDoor>();
        looking = GameObject.Find( "LitCamera" );

        allDoors.Add(this);
	}

    void OnDestroy()
    {
        allDoors.Remove(this);
    }
	
	void Update () 
	{
        if (isGrabbed)
        {

            Vector3 wantedPosition = looking.transform.position + looking.transform.forward * grabbedDistance;
            Vector3 diff = wantedPosition - door.transform.position;
            Vector3 reference = -door.transform.forward;

            float dot = Vector3.Dot(door.transform.right, (wantedPosition - door.transform.position).normalized);
            diff.y = 0;
            reference.y = 0;
            float angle = Vector3.Angle(diff, reference);


            Debug.Log(angle);

            if (dot < 0)
                door.CurrentSwing += Mathf.Min(angle, 300 * Time.deltaTime);
            else
                door.CurrentSwing -= Mathf.Min(angle, 300 * Time.deltaTime);
        }
	}
}
