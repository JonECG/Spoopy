using UnityEngine;
using System.Collections;

public class SoundTriggerable : Triggerable 
{
    public AudioClip soundToPlay;
    public Vector3 positionToPlayAt;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Triggered(string id)
    {
        SoundManagerController.Instance.PlaySoundAt(soundToPlay, positionToPlayAt);
    }
}
