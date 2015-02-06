using UnityEngine;
using System.Collections;

public class DeathHelper : MonoBehaviour {

    private static string[] message = { "" };
    private static float lastTimeRecorded = -50;

    private static float messageDuration = 14;
    private static float roomStart;

	void Start () 
	{
        roomStart = Time.time;
        Debug.Log(message[0]);
	}
	
	void Update () 
	{
        if (Time.time < roomStart + messageDuration)
        {
            if (lastTimeRecorded > roomStart - messageDuration)
            {
                float space = 0.1f;
                for (int i = 0; i < message.Length; i++)
                {
                    HeadsUpDisplayController.Instance.DrawText(message[i], 0, - space * i  + ( ( message.Length - 1 ) * space ) / 2 + 0.3f, Color.white, 0.05f);
                }
            }
        }
	}

    public static void RecordMessage(params string[] recordedMessage)
    {
        message = recordedMessage;
        lastTimeRecorded = Time.time;
    }
}
