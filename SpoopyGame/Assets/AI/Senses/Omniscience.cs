using UnityEngine;
using System.Collections;

public class Omniscience : SenseInterface {

    public override Brain.SensedInfo Sense()
    {
        Brain.SensedInfo result = new Brain.SensedInfo();
        GameObject player = GameObject.Find("Player");

        result.CertaintyIsPlayer = 1;
        result.CertaintyOfDistance = 1;
        result.CertaintyOfDirection = 1;
        result.AlertingFactor = 1; 
        result.SensedDirection = (player.transform.position - transform.position).normalized;
        result.SensedDistance = Vector3.Distance(player.transform.position, transform.position);

        return result;
    }
}
