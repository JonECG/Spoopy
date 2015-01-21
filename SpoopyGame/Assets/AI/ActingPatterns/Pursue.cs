using UnityEngine;
using System.Collections;

public class Pursue : ActingInterface
{
    Vector3 goHere;
    public float pursueSpeed;

    public override void Act(Brain.Perception perceived, Brain.Motivation motivation)
    {
        goHere = perceived.PerceivedWorldPosition;
        goHere.y = transform.position.y;

        transform.LookAt(goHere);
        transform.Translate((transform.forward.normalized * Mathf.Min( pursueSpeed * Time.deltaTime, Mathf.Max( Vector3.Distance( goHere, transform.position ) - 0.01f, 0 ) ) ), Space.World);
        //GetComponent<SoundStatePlayer>().SetState("Chase");
    }
}
