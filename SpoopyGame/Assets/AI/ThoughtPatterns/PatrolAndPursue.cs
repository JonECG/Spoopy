using UnityEngine;
using System.Collections;

public class PatrolAndPursue : ThoughtInterface {

    public ActingInterface patrolAction;
    public ActingInterface pursueAction;

    public float threshold = 0.5f;
    public override Brain.Motivation Think(Brain.Perception perceived)
    {
        Brain.Motivation motivate;

        motivate.Action = (perceived.Alertness > threshold) ? pursueAction : patrolAction;
        motivate.MotivationFactor = 1;

        return motivate;
    }
}
