using UnityEngine;
using System.Collections;

public class LightSensitive : SenseInterface {

    public override Brain.SensedInfo Sense()
    {
        Brain.SensedInfo result = new Brain.SensedInfo();

        HeadLamp lamp = FindObjectOfType<HeadLamp>();
        float dist = Vector3.Distance(lamp.transform.position, transform.position);
        bool isInLamp = lamp.light.enabled && dist < lamp.light.range && Mathf.Acos(Vector3.Dot((transform.position - lamp.transform.position).normalized, lamp.transform.forward)) < Mathf.Deg2Rad * lamp.light.spotAngle/2;

        if (isInLamp)
        {
            result.CertaintyIsPlayer = 1;
            result.CertaintyOfDistance = 1;
            result.CertaintyOfDirection = 1;
            result.AlertingFactor = 1;
            result.SensedDirection = (lamp.transform.position - transform.position).normalized;
            result.SensedDistance = dist;
        }

        return result;
    }
}
