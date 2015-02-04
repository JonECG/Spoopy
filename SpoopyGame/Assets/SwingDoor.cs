using UnityEngine;
using System.Collections;

public class SwingDoor : MonoBehaviour {

    public float SwingMin = 0;
    public float SwingMax = 90;
    public float ClosedThreshold = 1;
    public bool ClosableOnMin = true;
    public bool ClosableOnMax = true;
    private float closedCenter;

    public bool Locked = false;
    private bool closed = true;
    internal bool latched = true;

    public ColorCodeValues color;
    public ColorCode adjColor;

    public AudioClip unlockSound;
    public GameObject unlockEffect;

    public float CurrentSwing
    {
        get
        {
            return transform.localEulerAngles.y > 180 ? transform.localEulerAngles.y - 360 : transform.localEulerAngles.y;
        }
        set
        {
            float currentSwing = value;

            if (closed)
                currentSwing = Mathf.Clamp(currentSwing, closedCenter - ClosedThreshold, closedCenter + ClosedThreshold);
            else
                currentSwing = Mathf.Clamp(currentSwing, SwingMin, SwingMax);

            transform.localEulerAngles = new Vector3(0, currentSwing, 0);
        }
    }

	void Start () 
	{
        adjColor = ColorCode.FromValue(color);
        if (Locked)
        {
            transform.FindChild("DoorKnob").renderer.material.color = ColorCode.FromValue( color ).Color;
        }
	}
	
	void Update () 
	{
        if (Input.GetKey(KeyCode.O))
        {
            CurrentSwing -= 50 * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.P))
        {
            CurrentSwing += 50 * Time.deltaTime;
        }

        

        if (latched)
        {
            if (ClosableOnMin && CurrentSwing < SwingMin + ClosedThreshold * 2)
            {
                closedCenter = SwingMin + ClosedThreshold;
                closed = true;
            }
            if (ClosableOnMax && CurrentSwing > SwingMax - ClosedThreshold * 2)
            {
                closedCenter = SwingMax - ClosedThreshold;
                closed = true;
            }
        }
	}

    public void RequestLatch()
    {
        latched = true;
    }

    public void RequestUnlatch()
    {
        if (!Locked)
        {
            latched = false;
            closed = false;
        }
    }
}
