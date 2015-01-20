using UnityEngine;
using System.Collections;

public class Pursue : ActingInterface
{
    Vector3 goHere;
    public float pursueSpeed;

    public override void Act(Brain.Perception perceived, Brain.Motivation motivation)
    {
        goHere = perceived.PerceivedWorldPosition;

        transform.LookAt(new Vector3(goHere.x, transform.position.y, goHere.z));
        transform.Translate((transform.forward.normalized * pursueSpeed) * Time.deltaTime, Space.World);
        //GetComponent<SoundStatePlayer>().SetState("Chase");
    }
}
