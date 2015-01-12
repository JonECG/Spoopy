using UnityEngine;
using System.Collections;

public class Triggerer : MonoBehaviour {

    public Triggerable[] listeners;
    public bool active = true;
    public string message = "DEFAULT";

	void Start () 
	{
	
	}
	
	void Update () 
	{
	}

    void OnTriggerEnter(Collider other)
    {
        if ( active && other.GetComponent<PlayerController>() != null)
        {
            Debug.Log("Hit");
            for (int i = 0; i < listeners.Length; i++)
            {
                listeners[i].Triggered(message);
            }
            active = false;
        }
    }
}
