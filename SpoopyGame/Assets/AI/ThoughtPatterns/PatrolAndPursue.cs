using UnityEngine;
using System.Collections;

public class PatrolAndPursue : ThoughtInterface {

    public ActingInterface patrolAction;
    public ActingInterface pursueAction;

    public float threshold = 0.5f;

    private bool wasPursued = false;
    public override Brain.Motivation Think(Brain.Perception perceived)
    {
        Brain.Motivation motivate;

        bool isPursued = perceived.Alertness > threshold;

        if( isPursued )
            GetComponent<SoundStatePlayer>().SetState("Patrol");
        else
            GetComponent<SoundStatePlayer>().SetState("Chase");

        if( isPursued && !wasPursued )
            GetComponent<SoundStatePlayer>().PlaySoundFrom("FoundPlayer");

        motivate.Action = (perceived.Alertness > threshold) ? pursueAction : patrolAction;
        motivate.MotivationFactor = 1;

        wasPursued = isPursued;

        return motivate;
    }
}
