using UnityEngine;
using System.Collections;

public class MentalStability : MonoBehaviour {

    public delegate void CompletelyInsaneHandler();
    public event CompletelyInsaneHandler CompletelyInsaneEvent;

    public float insanity
    {
        get { return _insanity; }
        set
        {
            changed = value > _insanity;
            _insanity = Mathf.Clamp(value, 0, 1);
            if (_insanity == 1 && CompletelyInsaneEvent != null)
                CompletelyInsaneEvent();
        }
    }
    private float _insanity;
    
    private bool changed;
    public float sanityRefreshDuration = 5;

	void Start () 
	{
        _insanity = 0;
        changed = false;
	}
	
	void Update () 
	{
        if( !changed )
            insanity = Mathf.Max(0, insanity - Time.deltaTime / sanityRefreshDuration);
        changed = false;
	}
}
