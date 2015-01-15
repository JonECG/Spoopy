using UnityEngine;
using System.Collections;

public abstract class SenseInterface : MonoBehaviour {

    public float distance;
    public float acuteness;

    public abstract Brain.SensedInfo Sense();
}
