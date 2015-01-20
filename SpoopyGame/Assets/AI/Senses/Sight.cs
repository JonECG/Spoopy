using UnityEngine;
using System.Collections;

public class Sight : SenseInterface
{
    public override Brain.SensedInfo Sense()
    {
        Brain.SensedInfo si = new Brain.SensedInfo();

        GameObject player = GameObject.Find("Player");

        RaycastHit hit;

        if (Vector3.Distance(player.transform.position, transform.position) <= distance && Vector3.Angle(transform.forward, player.transform.position - transform.position) <= 90 && Physics.Raycast(transform.position+transform.up, player.transform.position - transform.position, out hit, distance))
        {
            if (hit.collider.name == "Player")
            {
                si.AlertingFactor = 1.0f;
                si.CertaintyIsPlayer = 1.0f;
                si.CertaintyOfDirection = 1.0f;
                si.CertaintyOfDistance = 0.8f;
                si.SensedDirection = player.transform.position - transform.position;
                si.SensedDistance = Vector3.Distance(player.transform.position, transform.position);
            }
        }
        else
        {
            si.AlertingFactor = 0.0f;
        }

        return si;
    }
}