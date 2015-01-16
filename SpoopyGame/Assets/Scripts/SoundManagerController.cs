using UnityEngine;
using System.Collections;

public class SoundManagerController : MonoBehaviour {


    public delegate void SoundCreatedHandler(AudioSource clip);
    public event SoundCreatedHandler SoundCreatedEvent;
    //Singleton static logic
    private static SoundManagerController instance = null;
    public static SoundManagerController Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    //Music transitioning
    private AudioClip nextMusic;

    public float fadeTime = 0.3f;
    public float volumeScale = 0.5f;

    bool isPlaying = false;
    bool isTransitioning = false;
    bool fadingOut;

    private float transitioningTime;


    //Public interface
    public void TransitionPlay(AudioClip music)
    {
        if (!isTransitioning)
        {
            if (music != null)
            {
                if (!isPlaying)
                {
                    audio.clip = music;
                    isPlaying = true;
                    audio.loop = true;
                    audio.volume = volumeScale;
                    audio.Play();
                    audio.time = 0;
                }
                else
                    if (music != audio.clip)
                    {
                        isTransitioning = true;
                        fadingOut = true;
                        transitioningTime = 0;
                        nextMusic = music;
                    }
            }
        }
    }

    public void TransitionPlay(string name)
    {
        if (!isTransitioning)
        {
            AudioClip loaded = Resources.Load<AudioClip>(name);

            TransitionPlay(loaded);
        }
    }

    public void TransitionStop()
    {

    }

    public void PlaySoundAt(string name, Vector3 soundPosition, string tag = "Untagged" )
    {
        AudioClip loaded = Resources.Load<AudioClip>(name);
        PlaySoundAt(loaded, soundPosition, tag);
    }

    public void PlaySoundAt(string name, Transform soundParent, string tag = "Untagged")
    {
        AudioClip loaded = Resources.Load<AudioClip>(name);
        PlaySoundAt(loaded, soundParent, tag);
    }

    public AudioSource PlaySoundAt(AudioClip clip, Vector3 soundPosition, string tag = "Untagged")
    {
        GameObject source = new GameObject(clip.name + "SOUNDED");
        source.AddComponent<AudioSource>();
        source.audio.clip = clip;
        source.transform.position = soundPosition;
        source.AddComponent<DestroyOnSoundEnd>();
        source.audio.Play();
        source.tag = tag;
        //Debug.Log( source.audio.spread );
        //source.audio.spread = 360;

        if( SoundCreatedEvent != null )
            SoundCreatedEvent(source.audio);

        return source.audio;
    }

    public AudioSource PlaySoundAt(AudioClip clip, Transform soundParent, string tag = "Untagged")
    {
        AudioSource source = PlaySoundAt(clip, soundParent.position, tag);
        source.transform.SetParent(soundParent, false);
        source.transform.localPosition = new Vector3(0, 0, 0);
        return source;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (isTransitioning)
        {
            transitioningTime += Time.deltaTime;

            if (fadingOut)
            {
                audio.volume = volumeScale * Mathf.Clamp((fadeTime - transitioningTime) / fadeTime, 0, 1);
            }
            else
            {
                audio.volume = volumeScale * (1 - Mathf.Clamp((fadeTime - transitioningTime) / fadeTime, 0, 1));
            }

            if (transitioningTime > fadeTime)
            {
                if (fadingOut)
                {
                    if (nextMusic != null)
                    {
                        audio.Stop();
                        audio.clip = nextMusic;
                        audio.time = 0;
                        audio.Play();
                        nextMusic = null;
                        fadingOut = false;
                    }
                    else
                    {
                        audio.Stop();
                        isPlaying = false;
                        isTransitioning = false;
                    }
                }
                else
                {
                    isTransitioning = false;
                }

                transitioningTime = 0;
            }
        }
	}



}
