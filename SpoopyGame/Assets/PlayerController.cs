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
        Application.LoadLevel(Application.loadedLevel);
	}
}
