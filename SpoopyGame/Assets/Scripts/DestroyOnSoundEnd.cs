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
        hasPlayed = hasPlayed || GetComponent<AudioSource>().isPlaying;
        if (hasPlayed && !GetComponent<AudioSource>().isPlaying)
        {
            Destroy(gameObject);
        }
	}
}
