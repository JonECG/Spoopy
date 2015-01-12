using UnityEngine;
using System.Collections;

public class HeadLamp : MonoBehaviour {

    public AudioClip recharge, rechargeFull, clickOn, clickOff, clickDead;
    public float batteryLifeInSeconds = 60;
    public float numberOfShakesToCharge = 6;
    public float weakenTime = 5;

    private float currentBatteryLife;
    private bool isTurnedOn;
    private float maxIntensity;

	void Start () 
	{
        isTurnedOn = false;
        currentBatteryLife = batteryLifeInSeconds;
        maxIntensity = light.intensity;
	}
	
	void Update () 
	{
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SoundManagerController.Instance.PlaySoundAt(isTurnedOn ? clickOff : (currentBatteryLife > 0) ? clickOn : clickDead, transform.position);
            if (currentBatteryLife > 0)
                isTurnedOn = !isTurnedOn;
        }

        if (isTurnedOn)
        {
            currentBatteryLife -= Time.deltaTime;

            if (currentBatteryLife <= 0)
            {
                currentBatteryLife = 0;
                isTurnedOn = false;
                SoundManagerController.Instance.PlaySoundAt(clickOff, transform.position);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            currentBatteryLife = Mathf.Min(batteryLifeInSeconds, currentBatteryLife + batteryLifeInSeconds / numberOfShakesToCharge);
            SoundManagerController.Instance.PlaySoundAt( ( currentBatteryLife == batteryLifeInSeconds ) ? rechargeFull : recharge, transform.position);
        }

        if (isTurnedOn)
        {
            float weakenedPower = (currentBatteryLife > weakenTime) ? 1 : currentBatteryLife / weakenTime;
            light.enabled = Random.value <= weakenedPower;
            light.intensity = weakenedPower * maxIntensity;
        }
        else
        {
            light.enabled = false;
        }
	}
}
