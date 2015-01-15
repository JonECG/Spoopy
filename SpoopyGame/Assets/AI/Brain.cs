using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Brain : MonoBehaviour {

    public float Reflexiveness = 1;
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

    void Start()
    {
        PerceivedWorldPosition = transform.position;
        PerceivedDistance = 0;
        PerceivedDirection = new Vector3(1, 0, 0);
    }

    private float SenseLerp(float start, float goal)
    {
        return start + (goal - start) * Reflexiveness * Time.deltaTime;
    }

    private Vector3 SenseLerp(Vector3 start, Vector3 goal)
    {
        return (start + (goal - start) * Reflexiveness * Time.deltaTime).normalized;
    }

    void CombineSenses()
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

        Alertness = SenseLerp(Alertness, (1 - heldInvAlertness));
        PerceivedDistance = SenseLerp(PerceivedDistance, heldDistance);
        PerceivedDirection = SenseLerp(PerceivedDirection, heldDirection);
        PerceivedWorldPosition = SenseLerp(PerceivedWorldPosition, heldDirection * heldDistance + transform.position);
        CertaintyIsPlayer = SenseLerp(CertaintyIsPlayer, (1 - heldInvPlayerCertainty));
        CertaintyOfDistance = SenseLerp(CertaintyOfDistance, (1 - heldInvDistanceCertainty));
        CertaintyOfDirection = SenseLerp(CertaintyOfDirection, (1 - heldInvDirectionCertainty));
    }

    void ThinkOnIt()
    {
        ThoughtInterface[] ideas = GetComponents<ThoughtInterface>();

        Perception p = new Perception() { Alertness = Alertness, CertaintyIsPlayer = CertaintyIsPlayer, CertaintyOfDirection = CertaintyOfDirection,
            CertaintyOfDistance = CertaintyOfDistance,PerceivedDirection = PerceivedDirection, PerceivedDistance = PerceivedDistance, PerceivedWorldPosition = PerceivedWorldPosition };

        foreach (ThoughtInterface idea in ideas)
        {
            idea.Think(p);
        }
    }

    void Update()
    {
        CombineSenses();
        ThinkOnIt();
    }
}
