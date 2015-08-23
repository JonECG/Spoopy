using UnityEngine;
using System.Collections;

public class DarkVision : MonoBehaviour {

    public LightDetector lightDetector;
    public Vector3 adjustedLight;

    private float maxIntensity;
    private float maxRange;

    public float grandeur;
    public float grandPower = 4;
    public float tweenResistance = 100;

	void Awake () 
	{
        maxIntensity = GetComponent<Light>().intensity;
        maxRange = GetComponent<Light>().range;
        adjustedLight = new Vector3(1, 1, 1);
	}
	
	void Update () 
	{
        adjustedLight = (adjustedLight * tweenResistance + lightDetector.averageColorAsVec) / (tweenResistance + 1);

        float darkness = 1 - ( lightDetector.averageColorAsVec.x + lightDetector.averageColorAsVec.y + lightDetector.averageColorAsVec.z ) / 3;
        grandeur = (Mathf.Pow(darkness, grandPower) + grandeur * tweenResistance) / (tweenResistance + 1);
        GetComponent<Light>().intensity = maxIntensity * grandeur;
        GetComponent<Light>().range = maxRange * grandeur;
	}
}
