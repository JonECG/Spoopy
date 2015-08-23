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

        GetComponent<ItemInteraction>().info = "A " + ColorCode.FromValue(color).Name.ToUpper() + " Key";
	}

    void OnCollisionEnter(Collision col)
    {
        SwingDoor swdoor = col.collider.GetComponentInParent<SwingDoor>();

        if (swdoor != null && swdoor.Locked)
        {
            if (swdoor.color == color)
            {
                swdoor.Locked = false;
                swdoor.unlockEffect.GetComponent<ParticleSystem>().Play();
                SoundManagerController.Instance.PlaySoundAt(swdoor.unlockSound, swdoor.transform);
            }
        }
    }
}
