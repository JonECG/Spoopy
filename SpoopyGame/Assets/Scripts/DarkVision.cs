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

	void Start () 
	{
        maxIntensity = light.intensity;
        maxRange = light.range;
        adjustedLight = new Vector3(1, 1, 1);
	}
	
	void Update () 
	{
        adjustedLight = (adjustedLight * tweenResistance + lightDetector.averageColorAsVec) / (tweenResistance + 1);

        float darkness = 1 - ( lightDetector.averageColorAsVec.x + lightDetector.averageColorAsVec.y + lightDetector.averageColorAsVec.z ) / 3;
        grandeur = (Mathf.Pow(darkness, grandPower) + grandeur * tweenResistance) / (tweenResistance + 1);
        light.intensity = maxIntensity * grandeur;
        light.range = maxRange * grandeur;
	}
}
