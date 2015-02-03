using UnityEngine;
using System.Collections;

public class StopBySense : ThoughtInterface {

    public SenseInterface stoppingSense;

    private SpikeyBits spike;

    public ActingInterface NotKnown, KnownButSenseStopped, Known;

    void Start()
    {
        spike = GetComponent<SpikeyBits>();
    }

    public override Brain.Motivation Think(Brain.Perception perceived)
    {
        Brain.Motivation mot = new Brain.Motivation();

        if (perceived.Alertness > 0.5)
        {
            if (stoppingSense.Sense().AlertingFactor > 0.5)
            {
                mot.Action = KnownButSenseStopped;
                spike.enabled = false;
            }
            else
            {
                mot.Action = Known;
                spike.enabled = true;
            }
        }
        else
        {
            mot.Action = NotKnown;
            spike.enabled = false;
        }

        mot.MotivationFactor = 1;

        return mot;
    }
}
