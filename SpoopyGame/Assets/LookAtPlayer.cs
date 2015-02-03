using UnityEngine;
using System.Collections;

public class LookAtPlayer : ActingInterface {

    public override void Act(Brain.Perception perceived, Brain.Motivation motivation)
    {
        Vector3 xz = perceived.PerceivedWorldPosition - transform.position;
        xz.y = 0;
        xz.Normalize();

        transform.LookAt(xz + transform.position);
    } 
}
