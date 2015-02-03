using UnityEngine;
using System.Collections;

public class DoNothing : ActingInterface
{
    void Start()
    {
    }

    public override void Act(Brain.Perception perceived, Brain.Motivation motivation)
    {
    }
}
