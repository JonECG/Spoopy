using UnityEngine;
using System.Collections;

public class DestroyOnSoundEnd : MonoBehaviour {

    private bool hasPlayed;

	void Start () 
	{
        hasPlayed = false;
	}
	
	void Update () 
	{
        hasPlayed = hasPlayed || audio.isPlaying;
        if (hasPlayed && !audio.isPlaying)
        {
            Destroy(gameObject);
        }
	}
}
