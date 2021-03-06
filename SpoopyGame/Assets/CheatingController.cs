﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CheatingController : MonoBehaviour {

    private bool isCheating = false;
    public GameObject path;

	void Start () 
	{
        IEnumerable<RoomInfo> r = CustomRooms.Rooms;
	}
	
	void Update () 
	{
        if (Input.GetKeyDown(KeyCode.J))
        {
            isCheating = !isCheating;
        }

        if (isCheating)
        {
            GameObject.Find("Player").GetComponent<MentalStability>().insanity = 0;
            GameObject.Find("Player").GetComponent<HealthyLiving>().health = 1;
            if (path != null)
                path.gameObject.SetActive(true);
        }
        else
        {
            if (path != null)
                path.gameObject.SetActive(false);
        }
	}
}
