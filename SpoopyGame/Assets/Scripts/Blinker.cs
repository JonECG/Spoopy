using UnityEngine;
using System.Collections;

public class Blinker : MonoBehaviour 
{
    public float BlinkLeftPercentage = 0.0f;
    public float BlinkRightPercentage = 0.0f;
    public float BlinkSpeed = 6.0f;
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        BlinkLeftPercentage = Mathf.Clamp(BlinkLeftPercentage + (Input.GetKey(KeyCode.E) ? 2 : -1) * BlinkSpeed * Time.deltaTime, 0, 1);
        BlinkRightPercentage = Mathf.Clamp(BlinkRightPercentage + (Input.GetKey(KeyCode.R) ? 2 : -1) * BlinkSpeed * Time.deltaTime, 0, 1);  
	}
}
