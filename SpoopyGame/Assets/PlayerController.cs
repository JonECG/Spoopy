using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public MentalStability stable;
    public HealthyLiving healthy;

	void Start () 
	{
        stable.CompletelyInsaneEvent += Death;
        healthy.NoHealthEvent += Death;
	}
	
	void Death () 
	{
        if (stable.insanity == 1)
            DeathHelper.RecordMessage("Looking at unsettling objects will make you go insane", "You can press (LT) or (RT) to close your eyes");
        Application.LoadLevel(Application.loadedLevel);
	}
}
