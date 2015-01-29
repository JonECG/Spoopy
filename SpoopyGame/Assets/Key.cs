using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour {

    public ColorCodeValues color;

	void Start () 
	{
        Renderer[] rends = GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in rends)
        {
            renderer.material.color = ColorCode.FromValue(color).Color;
        }
	}

    void OnCollisionEnter(Collision col)
    {
        SwingDoor swdoor = col.collider.GetComponentInParent<SwingDoor>();

        if (swdoor != null)
        {
            if (swdoor.color == color)
            {
                swdoor.Locked = false;
            }
        }
    }
}
