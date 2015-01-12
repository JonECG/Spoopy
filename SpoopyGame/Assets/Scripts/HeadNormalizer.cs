using UnityEngine;
using System.Collections;

public class HeadNormalizer : MonoBehaviour {

    public OVRManager riftHandle;
    public float unitsPerMeter = 1;
    public float multiplier = 1;
	void Start () 
    {
	}
	
	void Update () 
	{
        transform.localPosition = new Vector3(0, (riftHandle == null) ? 2 : OVRManager.profile.eyeHeight * multiplier);
        transform.localScale = new Vector3( multiplier, multiplier, multiplier );

        if( Input.GetKeyDown( KeyCode.O ) )
            multiplier *= 0.9f;
        if (Input.GetKeyDown(KeyCode.P))
            multiplier /= 0.9f;
	}
}
