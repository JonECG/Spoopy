using UnityEngine;
using System.Collections;

public class Sight : SenseInterface
{
    public float SightAngle = 60;
    public override Brain.SensedInfo Sense()
    {
        Brain.SensedInfo si = new Brain.SensedInfo();

        GameObject player = GameObject.Find("Player");

        float dist = (player.transform.position - transform.position).magnitude;

        if (dist <= distance && Vector3.Angle(transform.forward, player.transform.position - transform.position) <= SightAngle
            && !Physics.Raycast(transform.position, (player.transform.position - transform.position).normalized, dist, 1 << LayerMask.NameToLayer("Map")))
        {
            si.AlertingFactor = 1.0f;
            si.CertaintyIsPlayer = 1.0f;
            si.CertaintyOfDirection = 1.0f;
            si.CertaintyOfDistance = 1.0f;
            si.SensedDirection = (player.transform.position - transform.position).normalized;
            si.SensedDistance = dist;
        }
        else
        {
            si.AlertingFactor = 0.0f;
        }

        return si;
    }
}