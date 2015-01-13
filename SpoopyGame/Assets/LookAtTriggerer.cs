using UnityEngine;
using System.Collections;

public class LookAtTriggerer : Triggerer {

    public float angleThreshold = 45;
    public float maxDistance = 5;

    private Transform playerLooking;
    void Start()
    {
        playerLooking = GameObject.Find("LitCamera").transform;
    }

    void Update()
    {
        if( active )
        {
            Vector3 dir = (transform.position - playerLooking.position).normalized;
            if (active && Mathf.Acos( Vector3.Dot(playerLooking.forward, dir) ) * Mathf.Rad2Deg < angleThreshold)
            {
                if (!Physics.Raycast(playerLooking.position, dir, maxDistance, 0))
                {
                    SendTrigger();
                }
            }
        }
    }
}
