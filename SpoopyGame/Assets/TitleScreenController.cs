using UnityEngine;
using System.Collections;

public class TitleScreenController : MonoBehaviour {

    public GameObject oculCamera, normalCamera;
    public Texture2D one, two;

    private float waitStarted;
    public float waitTime = 5;

    Debouncer.DebouncerResults pressACorrected;

	void Start () 
	{
        GetComponent<Renderer>().material.mainTexture = one;
        waitStarted = 0;
        //Screen.fullScreen=true;
        if (Application.isEditor)
        {
            oculCamera.SetActive(false);
            normalCamera.SetActive(true);
        }
        else
        {
            oculCamera.SetActive(true);
            normalCamera.SetActive(false);
        }
	}
	
	void Update () 
	{
        pressACorrected = Debouncer.Debounce("Interact", pressACorrected);
        if (waitStarted == 0)
        {
            if (pressACorrected.IsPressed())
            {
                waitStarted = Time.time;
                GetComponent<Renderer>().material.mainTexture = two;
            }
        }
        else
        {
            if (waitStarted < Time.time - waitTime)
            {
                Application.LoadLevel(1);
            }
        }
	}
}
