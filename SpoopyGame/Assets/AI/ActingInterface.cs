using UnityEngine;
using System.Collections;

public abstract class ActingInterface : MonoBehaviour {

    public abstract void Act(Brain.Perception perceived, Brain.Motivation motivation);

}
