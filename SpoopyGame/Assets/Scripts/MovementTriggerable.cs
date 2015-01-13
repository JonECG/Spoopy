using UnityEngine;
using System.Collections;

public class MovementTriggerable : Triggerable
{
    private Vector3 startPosition;
    public Vector3 endPosition;
    private Vector3 direction;
    public float speedMultiplier;
    private bool moving = false;
	// Use this for initialization
	void Start () 
    {
        startPosition = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (moving)
        {
            this.transform.position += direction * speedMultiplier * Time.deltaTime;

            if ((transform.position - endPosition).magnitude < 1.0f)
            {
                //dissapear
                moving = false;
                if (renderer != null)
                    renderer.enabled = false;
            }

        }
	}

    public override void Triggered(string id)
    {
        moving = true;
        if (renderer != null)
            renderer.enabled = true;
        direction = (endPosition - startPosition).normalized;
    }
}
