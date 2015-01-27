using UnityEngine;
using System.Collections;

public class Auratic : SenseInterface
{
    public override Brain.SensedInfo Sense()
    {
        Brain.SensedInfo si=new Brain.SensedInfo();

        GameObject player = GameObject.Find("Player");

        RaycastHit hit;

        if (Vector3.Distance(player.transform.position, transform.position) <= distance && !Physics.Raycast(transform.position+transform.up, (player.transform.position - transform.position).normalized, out hit, Vector3.Distance(transform.position, player.transform.position), 1 << LayerMask.NameToLayer("Map")))
        {
            si.AlertingFactor = 1.0f;
            si.CertaintyIsPlayer = 1.0f;
            si.CertaintyOfDirection = 1.0f;
            si.CertaintyOfDistance = 1.0f;
            si.SensedDirection = (player.transform.position - transform.position).normalized;
            si.SensedDistance = Vector3.Distance(player.transform.position, transform.position);
        }
        else
        {
            si.AlertingFactor = 0.0f;
        }

        return si; 
    }
}
