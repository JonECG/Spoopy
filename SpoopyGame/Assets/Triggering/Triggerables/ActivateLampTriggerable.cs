using UnityEngine;
using System.Collections;

public class ActivateLampTriggerable : Triggerable {

    void Start()
    {
        HeadLamp hl = FindObjectOfType<HeadLamp>();
        hl.enabled = false;
    }

    public override void Triggered(string id)
    {
        HeadLamp hl = FindObjectOfType<HeadLamp>();
        hl.currentBatteryLife = hl.batteryLifeInSeconds;
        hl.enabled = true;
        Debug.Log("Headlamp activated");
        hl.TurnOn();
    }
}
