using UnityEngine;
using System.Collections;

public class SpikeyBits : MonoBehaviour {

    public float damage = 0.4f;
    public float knockback = 1;
    public float range = 1;
    private HealthyLiving target;

    public string[] hint = { "" };

	void Start () 
	{
        target = FindObjectOfType<HealthyLiving>();
	}
	
	void Update () 
	{
        if (Vector3.SqrMagnitude(transform.position - target.transform.position) < range * range)
        {
            if (!string.IsNullOrEmpty(hint[0]))
                DeathHelper.RecordMessage(hint);
            target.health -= damage;
        }
	}
}
