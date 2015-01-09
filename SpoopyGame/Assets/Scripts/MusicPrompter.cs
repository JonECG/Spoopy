using UnityEngine;
using System.Collections;

public class MusicPrompter : MonoBehaviour {

    public AudioClip loop;

	void Start () 
	{
        SoundManagerController.Instance.TransitionPlay(loop);
	}
}
