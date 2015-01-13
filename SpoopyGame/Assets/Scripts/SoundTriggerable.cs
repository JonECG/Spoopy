using UnityEngine;
using System.Collections;

public class SoundTriggerable : Triggerable 
{
    public AudioClip soundToPlay;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void Triggered(string id)
    {
        SoundManagerController.Instance.PlaySoundAt(soundToPlay, transform);
    }
}
