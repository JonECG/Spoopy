using UnityEngine;
using System.Collections;

public class MentalStability : MonoBehaviour {

    public float insanity;
    public float sanityRefreshDuration = 5;

	void Start () 
	{
        insanity = 0;
        //RenderTexture r = new RenderTexture(256, 256, 16);
        //r.Create();
        //Camera.allCameras[0].targetTexture = r;
	}
	
	void Update () 
	{
        insanity = Mathf.Max(0, insanity - Time.deltaTime / sanityRefreshDuration);
        insanity = Mathf.Min(1, insanity);

        //insanity = ( Mathf.Sin(Time.time) + 1 ) / 2;
	}
}
