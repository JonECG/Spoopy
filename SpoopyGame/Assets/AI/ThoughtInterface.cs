using UnityEngine;
using System.Collections;

public abstract class ThoughtInterface : MonoBehaviour {

    public abstract Brain.Motivation Think(Brain.Perception perceived);

}
