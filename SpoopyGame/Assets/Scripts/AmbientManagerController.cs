using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
            timesPlayed[i] = Random.Range(0, ambientSounds[i].maxDelay );
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
                AudioListener listener = FindObjectOfType<AudioListener>();
                Vector3 forward = listener.transform.rotation * new Vector3( 1, 0, 0 );
                forward.y = 0;
                forward.Normalize();

                List<Vector2> angleRanges = new List<Vector2>();
                if (ambientSounds[i].playFront)
                    angleRanges.Add(new Vector2(-45, 45));
                if (ambientSounds[i].playBehind)
                    angleRanges.Add(new Vector2(135, 225));
                if (ambientSounds[i].playSides)
                {
                    angleRanges.Add(new Vector2(45, 135));
                    angleRanges.Add(new Vector2(225, 315));
                }

                Vector2 chosenRange = angleRanges[Random.Range(0, angleRanges.Count)];
                float angle = Random.Range(chosenRange.x, chosenRange.y);
                float dist = Random.Range(ambientSounds[i].minDistance, ambientSounds[i].maxDistance);
                Vector3 direction = Quaternion.AngleAxis(angle, new Vector3(0, 1, 0)) * forward;
                Vector3 soundPosition = listener.transform.position + direction * dist;

                SoundManagerController.Instance.PlaySoundAt(ambientSounds[i].sound, soundPosition);
                timesPlayed[i] = Random.Range(ambientSounds[i].minDelay, ambientSounds[i].maxDelay) + ambientSounds[i].sound.length;
            }
        }
	}
}
