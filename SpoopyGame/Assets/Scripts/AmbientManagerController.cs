using UnityEngine;
using System.Collections;

public class AmbientManagerController : MonoBehaviour {

    [System.Serializable]
    public class AmbientSoundInfo
    {
        public AudioClip sound;
        public float minDistance = 2;
        public float maxDistance = 6;
        public float minDelay = 10;
        public float maxDelay = 60;
        public bool playFront = false;
        public bool playBehind = true;
        public bool playSides = true;
    }

    [SerializeField]
    private AmbientSoundInfo[] ambientSounds;
    private float[] timesPlayed;

	// Use this for initialization
	void Start () 
    {
        timesPlayed = new float[ambientSounds.Length];
        for (int i = 0; i < timesPlayed.Length; i++)
        {
            timesPlayed[i] = Random.Range(0, ambientSounds[i].maxDelay) + ambientSounds[i].sound.length;
        }
	}

    // Update is called once per frame
    void Update() 
    {
        for (int i = 0; i < timesPlayed.Length; i++)
        {
            timesPlayed[i] -= Time.deltaTime;
            if (timesPlayed[i] < 0)
            {
                Vector3 playerPosition = GameObject.Find("Player").transform.position;
                SoundManagerController.Instance.PlaySoundAt( ambientSounds[i].sound, playerPosition );
                timesPlayed[i] = Random.Range(ambientSounds[i].minDelay, ambientSounds[i].maxDelay) + ambientSounds[i].sound.length;
            }
        }
	}
}
