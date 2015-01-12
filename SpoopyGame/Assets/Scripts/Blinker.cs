using UnityEngine;
using System.Collections;

public class Blinker : MonoBehaviour 
{
    public float BlinkLeftPercentage = 0.0f;
    public float BlinkRightPercentage = 0.0f;
    private float blinkspeed = 6.0f;
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKey(KeyCode.E))
        {
            if (BlinkLeftPercentage <= 1.0f)
            {
                BlinkLeftPercentage += blinkspeed * Time.deltaTime;
            }
            else
            {
                BlinkLeftPercentage = 1.0f;
            }
        }
        else
        {
            if (BlinkLeftPercentage > 0.0f)
            {
                BlinkLeftPercentage -= blinkspeed * Time.deltaTime;
            }
            else
            {
                BlinkLeftPercentage = 0.0f;
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (BlinkRightPercentage <= 1.0f)
            {
                BlinkRightPercentage += blinkspeed * Time.deltaTime;
            }
            else
            {
                BlinkRightPercentage = 1.0f;
            }
        }
        else
        {
            if (BlinkRightPercentage > 0.0f)
            {
                BlinkRightPercentage -= blinkspeed * Time.deltaTime;
            }
            else
            {
                BlinkRightPercentage = 0.0f;
            }
        }
	}
}
