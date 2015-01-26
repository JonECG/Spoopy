using UnityEngine;
using System.Collections;

public class MovementTriggerable : Triggerable
{
    private Vector3 startPosition;
    public Vector3 EndPosition;
    public Triggerable OnMovementEnd;
    private Vector3 direction;
    public float Speed;
    private bool moving = false;
    private string heldMessage;
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
            if ((transform.position - EndPosition).magnitude < Speed * Time.deltaTime)
            {
                moving = false;
                if (OnMovementEnd != null)
                    OnMovementEnd.Triggered(heldMessage);
            }
            else
            {
                this.transform.position += direction * Speed * Time.deltaTime;
            }
        }
	}

    public override void Triggered(string id)
    {
        heldMessage = id;
        startPosition = transform.position;
        moving = true;
        direction = (EndPosition - startPosition).normalized;
    }
}
