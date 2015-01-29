using UnityEngine;
using System.Collections;

public class TakeItemTriggerer : Triggerer
{
    //Actually doesn't need anything
    void Start()
    {
    }

    public void Take()
    {
        SendTrigger();
    }
}
