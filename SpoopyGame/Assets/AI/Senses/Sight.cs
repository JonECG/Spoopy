using UnityEngine;
using System.Collections;

public class Sight : SenseInterface
{
    public override Brain.SensedInfo Sense()
    {
        Brain.SensedInfo si = new Brain.SensedInfo();

        GameObject player = GameObject.Find("Player");

        if (Vector3.Distance(player.transform.position, transform.position) <= distance && !Physics.Raycast(transform.position, player.transform.position - transform.position, Vector3.Distance(transform.position, player.transform.position)) && Vector3.Angle(transform.forward, player.transform.position - transform.position) < 180)
        {
            si.AlertingFactor = 1.0f;
            si.CertaintyIsPlayer = 1.0f;
            si.CertaintyOfDirection = 1.0f;
            si.CertaintyOfDistance = 0.8f;
            si.SensedDirection = player.transform.position - transform.position;
            si.SensedDistance = Vector3.Distance(player.transform.position, transform.position);
        }
        else
        {
            si.AlertingFactor = 0.0f;
        }

        return si;
    }
}