using UnityEngine;
using System.Collections;

public class OverrideSpecialMessage : MonoBehaviour {

    public string[] message = { "" };
	void Start () 
	{
        DeathHelper.RecordMessage(message);
	}
	
}
