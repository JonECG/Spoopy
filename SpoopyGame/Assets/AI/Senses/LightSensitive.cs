using UnityEngine;
using System.Collections;

public class LightSensitive : SenseInterface {

    public override Brain.SensedInfo Sense()
    {
        Brain.SensedInfo result = new Brain.SensedInfo();

        HeadLamp lamp = FindObjectOfType<HeadLamp>();
        float dist = Vector3.Distance(lamp.transform.position, transform.position);
        bool isInLamp = lamp.GetComponent<Light>().enabled && dist < lamp.GetComponent<Light>().range && Mathf.Acos(Vector3.Dot((transform.position - lamp.transform.position).normalized, lamp.transform.forward)) < Mathf.Deg2Rad * lamp.GetComponent<Light>().spotAngle/2 &&
            !Physics.Raycast( lamp.transform.position, (transform.position - lamp.transform.position).normalized, dist, 1 << LayerMask.NameToLayer("Map") );

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
