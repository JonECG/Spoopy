using UnityEngine;
using System.Collections;
using System;

public class TimedTriggerables : Triggerable
{
    [Serializable]
    public struct TimedEvent
    {
        public float Time;
        public Triggerable Event;
        internal bool executed;
    }

    public TimedEvent[] events;

    private bool executing;
    private float currentTime;
    private string heldMessage;

	void Start () 
	{
        currentTime = 0;

        for (int i = 0; i < events.Length; i++)
        {
            events[i].executed = false;
        }
	}

    void Update() 
	{
        if (executing)
        {
            bool completed = true;
            for (int i = 0; i < events.Length; i++)
            {
                if (currentTime >= events[i].Time && !events[i].executed)
                {
                    events[i].executed = true;
                    events[i].Event.Triggered(heldMessage);
                }
                completed = completed && events[i].executed;
            }

            currentTime += Time.deltaTime;

            if (completed)
                executing = false;
        }
	}

    public override void Triggered(string id)
    {
        heldMessage = id;
        currentTime = 0;
        executing = true;
    }
}
