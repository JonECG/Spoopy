using UnityEngine;
using System.Collections;

public class SpikeyBits : MonoBehaviour {

    public float damage = 0.4f;
    public float knockback = 1;
    public float range = 1;
    private HealthyLiving target;

	void Start () 
	{
        target = FindObjectOfType<HealthyLiving>();
	}
	
	void Update () 
	{
        if (Vector3.SqrMagnitude(transform.position - target.transform.position) < range * range)
        {
            target.health -= damage;
        }
	}
}
