using UnityEngine;
using System.Collections;

public class LampDischargeTriggerable : Triggerable {

    public override void Triggered(string id)
    {
        HeadLamp hl = FindObjectOfType<HeadLamp>();
        hl.currentBatteryLife = hl.weakenTime;
    }
}
