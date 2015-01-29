using UnityEngine;
using System.Collections;

public class CollisionTriggerer : Triggerer {

    void OnTriggerEnter(Collider other)
    {
        if ( active && other.GetComponent<PlayerController>() != null)
        {
            SendTrigger();
            collider.enabled = false;
        }
    }
}
