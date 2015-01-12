using UnityEngine;
using System.Collections;

public class MovementTriggerable : Triggerable
{
    private Vector3 startPosition;
    public Vector3 endPosition;
    public float speedMultiplier;
    private bool moving = false;
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (moving)
        {
            startPosition = this.transform.position;
            transform.LookAt(endPosition);

            this.transform.Translate(transform.forward.normalized * speedMultiplier * Time.deltaTime);

            if ((transform.position - endPosition).magnitude < 1.0f)
            {
                //dissapear
                moving = false;
            }

        }
	}

    public void Triggered(string id)
    {
        moving = true;
    }
}
