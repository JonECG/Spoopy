using UnityEngine;
using System.Collections;

public class Seen : SenseInterface {

    public override Brain.SensedInfo Sense()
    {
        Brain.SensedInfo result = new Brain.SensedInfo();

        LightDetector light = FindObjectOfType<LightDetector>();
        Color seenPixel;
        if (light.CanSee(gameObject, out seenPixel))
        {
            float brightness = Mathf.Min( (seenPixel.r + seenPixel.g + seenPixel.b) / 2, 1 );
            result.CertaintyIsPlayer = brightness;
            result.CertaintyOfDistance = brightness;
            result.CertaintyOfDirection = brightness;
            result.AlertingFactor = Mathf.Min( 1, brightness*10 );
            result.SensedDirection = (light.transform.position - transform.position).normalized;
            result.SensedDistance = (light.transform.position - transform.position).magnitude;
        }

        return result;
    }
}
