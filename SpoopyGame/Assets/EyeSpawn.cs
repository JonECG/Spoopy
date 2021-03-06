﻿using UnityEngine;
using System.Collections;

public class EyeSpawn : MonoBehaviour {

    public GameObject[] eye;
    public float xOff;
    public float yOff;
    public float zOff;

	// Use this for initialization
	void Start ()
    {
        for (int i = 0; i < eye.Length; i++)
        {
            float x=transform.position.x+(Random.Range(-xOff, xOff));
            float y=transform.position.y+(Random.Range(-yOff, yOff));
            float z=transform.position.z+(Random.Range(-zOff, zOff));
            eye[i].transform.position = new Vector3(x, y, z);
        }
	}
}
