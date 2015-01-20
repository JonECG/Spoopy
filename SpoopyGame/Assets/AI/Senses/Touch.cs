using UnityEngine;
using System.Collections;

public class Touch : SenseInterface {

    bool isTouching;

    void Start()
    {
        isTouching = false;
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.GetComponent<PlayerController>() != null)
        {
            isTouching = true;
        }
    }

    void OnTriggerExit(Collider c)
    {
        if (c.gameObject.GetComponent<PlayerController>() != null)
        {
            isTouching = false;
        }
    }

    public override Brain.SensedInfo Sense()
    {
        Brain.SensedInfo result = new Brain.SensedInfo();

        Debug.Log("Touching: " + isTouching);
        if( isTouching )
        {
            GameObject player = GameObject.Find("Player");

            result.CertaintyIsPlayer = 1;
            result.CertaintyOfDistance = 1;
            result.CertaintyOfDirection = 1;
            result.AlertingFactor = 1;
            result.SensedDirection = (player.transform.position - transform.position).normalized;
            result.SensedDistance = Vector3.Distance(player.transform.position, transform.position);
        }

        //isTouching = false;

        return result;
    }
}
