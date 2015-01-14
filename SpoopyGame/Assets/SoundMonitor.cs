using UnityEngine;
using System.Collections;

public class SoundMonitor : MonoBehaviour {

    public AudioClip ringing;
    public MentalStability stable;

    public AudioClip heartBeat;
    public HealthyLiving healthy;

    private AudioSource ringSource, beatSource;

    private float tweenedHealth = 1;
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
        
        tweenedHealth = (10*tweenedHealth + healthy.health) / 11;
        //tweenedHealth = Mathf.Min( 1, Input.mousePosition.x / Screen.width );
        ringSource.volume = stable.insanity * stable.insanity;
        beatSource.volume = Mathf.Sqrt(1 - tweenedHealth) + 0.2f;
        beatSource.pitch = (3 - tweenedHealth*2) / 1.5f;
	}
}
