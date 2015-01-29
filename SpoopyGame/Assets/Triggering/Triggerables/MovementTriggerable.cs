using UnityEngine;
using System.Collections;

public class MovementTriggerable : Triggerable
{
    //private Vector3 startPosition;
    public Vector3 PositionOffset;
    //private Vector3 endPosition;
    public Triggerable OnMovementEnd;
    private Vector3 direction;
    private float distTraveled;
    public float Speed;
    private bool moving = false;
    private string heldMessage;
	// Use this for initialization
	void Start () 
    {
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (moving)
        {
            if (distTraveled >= PositionOffset.magnitude)
            {
                moving = false;
                if (OnMovementEnd != null)
                    OnMovementEnd.Triggered(heldMessage);
            }
            else
            {
                float toMove = Mathf.Min(PositionOffset.magnitude - distTraveled, Speed * Time.deltaTime);
                this.transform.position += direction * toMove;
                distTraveled += toMove;
            }
        }
	}

    public override void Triggered(string id)
    {
        heldMessage = id;
        distTraveled = 0;
        moving = true;
        direction = (transform.root.rotation * PositionOffset).normalized;
    }
}
