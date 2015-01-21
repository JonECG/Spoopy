using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Brain : MonoBehaviour {

    public float Reflexiveness = 1;
    public float Stubbornness = 0;
    public float AttentionLossTime = 1.0f;
    private Motivation lastMotivation;
    private Vector3 lastPosition;

    public float CertaintyOfDirection { get; protected set; }
    public float CertaintyOfDistance { get; protected set; }
    public float CertaintyIsPlayer { get; protected set; }
    public Vector3 PerceivedWorldPosition { get; protected set; }
    public Vector3 PerceivedDirection { get; protected set; }
    public float PerceivedDistance { get; protected set; }
    public float Alertness { get; protected set; }

    public struct SensedInfo
    {
        public Vector3 SensedDirection;
        public float SensedDistance;
        public float CertaintyOfDirection;
        public float CertaintyOfDistance;
        public float CertaintyIsPlayer;
        public float AlertingFactor;
    }

    public struct Perception
    {
        public Vector3 PerceivedWorldPosition;
        public Vector3 PerceivedDirection;
        public float PerceivedDistance;
        public float CertaintyOfDirection;
        public float CertaintyOfDistance;
        public float CertaintyIsPlayer;
        public float Alertness;
    }

    public struct Motivation
    {
        public float MotivationFactor;
        public ActingInterface Action;
    }

    void Start()
    {
        PerceivedWorldPosition = transform.position;
        PerceivedDistance = 0;
        PerceivedDirection = new Vector3(1, 0, 0);
    }

    private float SenseLerp(float start, float goal, float factor = 1.0f )
    {
        return start + (goal - start) * Reflexiveness * Time.deltaTime * factor;
    }

    private Vector3 SenseLerp(Vector3 start, Vector3 goal, bool normalized, float factor = 1.0f)
    {
        Vector3 lerped = (start + (goal - start) * Reflexiveness * Time.deltaTime * factor);
        return (normalized) ? lerped.normalized : lerped;
    }

    Perception CombineSenses()
    {
        SenseInterface[] senses = GetComponents<SenseInterface>();
        Vector3 heldDirection = new Vector3();
        float heldDistance = 0;

        float heldInvDistanceCertainty = 1;
        float heldInvDirectionCertainty = 1;
        float heldInvPlayerCertainty = 1;
        float heldInvAlertness = 1;

        foreach (SenseInterface sense in senses)
        {
            SensedInfo sensed = sense.Sense();

            heldDirection += sensed.CertaintyOfDirection * sensed.SensedDirection;
            heldDistance += sensed.CertaintyOfDistance * sensed.SensedDistance;
            heldInvDistanceCertainty *= 1 - sensed.CertaintyOfDistance;
            heldInvDirectionCertainty *= 1 - sensed.CertaintyOfDirection;
            heldInvPlayerCertainty *= 1 - sensed.CertaintyIsPlayer;
            heldInvAlertness *= 1 - sensed.AlertingFactor;
        }

        heldDirection /= senses.Length;
        heldDirection.Normalize();
        heldDistance /= senses.Length;

        Alertness = Mathf.Max( 0, Alertness - Time.deltaTime / AttentionLossTime, (1 - heldInvAlertness) );
        PerceivedDistance = SenseLerp(PerceivedDistance, heldDistance, 1 - heldInvDistanceCertainty);
        PerceivedDirection = SenseLerp(PerceivedDirection, heldDirection, true, 1 - heldInvDirectionCertainty);
        PerceivedWorldPosition = SenseLerp(PerceivedWorldPosition, PerceivedDirection * PerceivedDistance + transform.position, false );
        CertaintyIsPlayer = SenseLerp(CertaintyIsPlayer, (1 - heldInvPlayerCertainty));
        CertaintyOfDistance = SenseLerp(CertaintyOfDistance, (1 - heldInvDistanceCertainty));
        CertaintyOfDirection = SenseLerp(CertaintyOfDirection, (1 - heldInvDirectionCertainty));

        Perception p = new Perception()
        {
            Alertness = Alertness,
            CertaintyIsPlayer = CertaintyIsPlayer,
            CertaintyOfDirection = CertaintyOfDirection,
            CertaintyOfDistance = CertaintyOfDistance,
            PerceivedDirection = PerceivedDirection,
            PerceivedDistance = PerceivedDistance,
            PerceivedWorldPosition = PerceivedWorldPosition
        };

        return p;
    }

    Motivation ThinkOnIt(Perception perceived)
    {
        ThoughtInterface[] ideas = GetComponents<ThoughtInterface>();

        Motivation largestMotivation = new Motivation() { Action = null, MotivationFactor = 0 };

        foreach (ThoughtInterface idea in ideas)
        {
            Motivation ideaMotivation = idea.Think(perceived);
            if (ideaMotivation.MotivationFactor > largestMotivation.MotivationFactor)
                largestMotivation = ideaMotivation;
        }

        return largestMotivation;
    }

    private void AccountForMovement()
    {
        Vector3 delta = transform.position - lastPosition;
        lastPosition = transform.position;

        Vector3 perceivedTotal = PerceivedDirection * PerceivedDistance;
        perceivedTotal -= delta;

        PerceivedDirection = perceivedTotal.normalized;
        PerceivedDistance = perceivedTotal.magnitude;
    }

    void Update()
    {
        AccountForMovement();
        Perception perception = CombineSenses();
        Motivation motivation = ThinkOnIt( perception );

        if (motivation.MotivationFactor < lastMotivation.MotivationFactor)
            motivation = lastMotivation;

        if (motivation.MotivationFactor > 0 && motivation.Action != null)
        {
            motivation.Action.Act(perception, motivation);
        }

        lastMotivation = motivation;
        lastMotivation.MotivationFactor *= Stubbornness;
    }
}
