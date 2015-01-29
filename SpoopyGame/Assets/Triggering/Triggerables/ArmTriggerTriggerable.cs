using UnityEngine;
using System.Collections;

public class ArmTriggerTriggerable : Triggerable {

    public Triggerer trig;
    void Start()
    {
    }

    public override void Triggered(string id)
    {
        trig.active = true;
    }
}
