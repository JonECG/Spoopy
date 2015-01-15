using UnityEngine;
using System.Collections;

public class Auratic : SenseInterface
{
    public override Brain.SensedInfo Sense()
    {
        Brain.SensedInfo si=new Brain.SensedInfo();

        GameObject player = GameObject.Find("Player");

        if (Vector3.Distance(player.transform.position, transform.position) <= distance && !Physics.Raycast(transform.position, player.transform.position - transform.position, Vector3.Distance(transform.position, player.transform.position)))
        {
            si.AlertingFactor = 1.0f;
            si.CertaintyIsPlayer = 1.0f;
            si.CertaintyOfDirection = 1.0f;
            si.CertaintyOfDistance = 1.0f;
            si.SensedDirection = player.transform.position - transform.position;
            si.SensedDistance = Vector3.Distance(player.transform.position, transform.position);
        }
        else
        {
            si.AlertingFactor = 0.0f;
            si.CertaintyIsPlayer = 0.0f;
            si.CertaintyOfDirection = 0.0f;
            si.CertaintyOfDistance = 0.0f;
            si.SensedDirection = new Vector3(0.0f, 0.0f, 0.0f);
            si.SensedDistance = 0;
        }

        return si;
    }
}
