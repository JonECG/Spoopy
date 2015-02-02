using UnityEngine;
using System.Collections;

public class LevelChangeTriggerable : Triggerable{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void Triggered(string id)
    {
        this.GetComponent<LevelController>().activateNewLevel();
    }
}
