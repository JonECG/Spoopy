using UnityEngine;
using System.Collections;

public class LookedToward : SenseInterface {

    public float angle = 90;

    public override Brain.SensedInfo Sense()
    {
        Brain.SensedInfo result = new Brain.SensedInfo();

        LightDetector light = FindObjectOfType<LightDetector>();

        if (Vector3.Angle(light.transform.forward, transform.position - light.transform.position) < angle)
        {
            result.CertaintyIsPlayer = 1;
            result.CertaintyOfDistance = 1;
            result.CertaintyOfDirection = 1;
            result.AlertingFactor = 1;
            result.SensedDirection = (light.transform.position - transform.position).normalized;
            result.SensedDistance = (light.transform.position - transform.position).magnitude;
        }

        return result;
    }
}
