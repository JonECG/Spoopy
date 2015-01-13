using UnityEngine;
using System.Collections;

public class LookAtTriggerer : Triggerer {

    public float angleThreshold = 45;
    public float maxDistance = 5;

    private Transform playerLooking;
    void Start()
    {
        playerLooking = GameObject.Find("LitCamera").transform;
        renderer.enabled = false;
    }

    void Update()
    {
        if( active )
        {
            float dist = (transform.position - playerLooking.position).magnitude;
            Vector3 dir = (transform.position - playerLooking.position).normalized;
            if (dist < maxDistance && Mathf.Acos(Vector3.Dot(playerLooking.forward, dir)) * Mathf.Rad2Deg < angleThreshold)
            {
                RaycastHit hit = new RaycastHit();
                if (!Physics.Raycast(playerLooking.position, dir, out hit, maxDistance, 1 << LayerMask.NameToLayer("Map")))
                {
                    Debug.Log("ASADLKSAJD:LKDSLSAKJDLKSADJ:LKSADJASDKDKJSAHDKHSADKLSAHD");
                    SendTrigger();
                }
                else
                {
                    if (hit.distance > dist)
                    {
                        Debug.Log("TYTYTYTYTYTYTYTYTUTUTIUIRIROIROREOREOREO");
                        SendTrigger();
                    }
                }
            }
        }
    }
}
