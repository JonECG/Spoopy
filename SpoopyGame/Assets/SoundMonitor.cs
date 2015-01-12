using UnityEngine;
using System.Collections;

public class SoundMonitor : MonoBehaviour {

    public AudioClip ringing;
    public MentalStability stable;

    public AudioClip heartBeat;
    public HealthyLiving healthy;

    private AudioSource ringSource, beatSource;

	void Start () 
	{
        ringSource = gameObject.AddComponent<AudioSource>();
        beatSource = gameObject.AddComponent<AudioSource>();

        ringSource.clip = ringing;
        ringSource.loop = true;
        ringSource.volume = 0;
        ringSource.Play();

        beatSource.clip = heartBeat;
        beatSource.loop = true;
        beatSource.volume = 0;
        beatSource.Play();
	}
	
	void Update () 
	{
        ringSource.volume = stable.insanity;
        beatSource.volume = Mathf.Sqrt( 1 - healthy.health );
        beatSource.pitch = (3 - healthy.health) / 2.0f;
	}
}
