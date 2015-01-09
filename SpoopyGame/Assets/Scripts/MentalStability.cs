using UnityEngine;
using System.Collections;

public class MentalStability : MonoBehaviour {

    public float insanity;
    public float sanityRefreshDuration = 5;

	void Start () 
	{
        insanity = 0;
	}
	
	void Update () 
	{
        insanity = Mathf.Max(0, insanity - Time.deltaTime / sanityRefreshDuration);
        insanity = Mathf.Min(1, insanity);
	}
}
