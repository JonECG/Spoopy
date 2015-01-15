using UnityEngine;
using System.Collections;

public abstract class ThoughtInterface : MonoBehaviour {

    public abstract void Think(Brain.Perception perceived);

}
