using UnityEngine;
using System.Collections;

public class SoundTriggerable : Triggerable 
{
    public AudioClip soundToPlay;

    public bool loop = false;
    public float loopOffset = 1;

    private bool isLooping = false;
    private float time = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (isLooping)
        {
            time += Time.deltaTime;
            if (time > loopOffset)
            {
                time -= loopOffset;
                SoundManagerController.Instance.PlaySoundAt(soundToPlay, transform);
            }
        }
	}

    public override void Triggered(string id)
    {
        SoundManagerController.Instance.PlaySoundAt(soundToPlay, transform);
        if (loop)
        {
            isLooping = true;
            time = 0;
        }
    }
}
