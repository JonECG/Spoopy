using UnityEngine;
using System.Collections;

public class SoundStatePlayer : MonoBehaviour {

    [System.Serializable]
    public class SoundStateInfo
    {
        public string name;
        public AudioClip[] sounds;
        public float minDelay = 2;
        public float maxDelay = 5;
        internal float barredTime;
    }

    [SerializeField]
    private SoundStateInfo[] soundStates;

    private SoundStateInfo currentState;
    private float timeTilPlay;

	void Start () 
	{
        currentState = null;
	}
	
	void Update () 
	{
        if (currentState != null)
        {
            timeTilPlay -= Time.deltaTime;
            if (timeTilPlay < 0)
            {
                timeTilPlay = Random.Range(currentState.minDelay, currentState.maxDelay);
                PlaySoundFrom(currentState);
            }
        }
	}

    private SoundStateInfo getState(string name)
    {
        SoundStateInfo found = null;
        for (int i = 0; i < soundStates.Length && found == null; i++)
        {
            if (name == soundStates[i].name)
            {
                found = soundStates[i];
            }
        }

        return found;
    }

    public void SetState(string name)
    {
        SoundStateInfo found = getState(name);
        if (found == null)
            Debug.Log("Sound state [" + name + "] does not exist");
        else
        {
            if( currentState == null )
                timeTilPlay = Random.Range(found.minDelay, found.maxDelay);
            currentState = found;
        }
    }

    public void PlaySoundFrom(string state)
    {
        SoundStateInfo found = getState(state);
        if (found == null)
            Debug.Log("Sound state [" + state + "] does not exist");
        else
            PlaySoundFrom(found);
    }

    private void PlaySoundFrom(SoundStateInfo state)
    {
        if (Time.time > state.barredTime)
        {
            AudioClip clip = state.sounds[Random.Range(0, state.sounds.Length)];
            SoundManagerController.Instance.PlaySoundAt(clip, transform);
            state.barredTime = clip.length + Time.time;
            timeTilPlay += clip.length;
        }
    }
}
