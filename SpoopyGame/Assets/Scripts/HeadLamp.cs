using UnityEngine;
using System.Collections;

public class HeadLamp : MonoBehaviour {

    public AudioClip recharge, rechargeFull, clickOn, clickOff, clickDead;
    public float batteryLifeInSeconds = 60;
    public float numberOfShakesToCharge = 6;
    public float weakenTime = 5;
    public float debounceThreshhold = 0.1f;

    private float debounce = 0.1f;
    private float currentBatteryLife;
    private bool isTurnedOn;
    private float maxIntensity;
    private bool debouncing = false;
	void Start () 
	{
        isTurnedOn = false;
        currentBatteryLife = batteryLifeInSeconds;
        maxIntensity = light.intensity;
	}
	
	void Update () 
	{

        if (debounce >= debounceThreshhold || !debouncing)
        {
            debouncing = false;
            if (Input.GetAxis("HeadLampToggle") > 0.0f)
            {
                SoundManagerController.Instance.PlaySoundAt(isTurnedOn ? clickOff : (currentBatteryLife > 0) ? clickOn : clickDead, transform.position);
                if (currentBatteryLife > 0)
                    isTurnedOn = !isTurnedOn;
                debouncing = true;
                debounce = 0.0f;
            }

            if (Input.GetAxis("HeadLampRecharge") > 0.0f)
            {
                currentBatteryLife = Mathf.Min(batteryLifeInSeconds, currentBatteryLife + batteryLifeInSeconds / numberOfShakesToCharge);
                SoundManagerController.Instance.PlaySoundAt((currentBatteryLife == batteryLifeInSeconds) ? rechargeFull : recharge, transform.position);
                debouncing = true;
                debounce = 0.0f;
            }
        }
        else if (debouncing)
        {
            debounce += Time.deltaTime;
        }
        else if (Input.GetAxis("HeadLampToggle") < 0.1f && Input.GetAxis("HeadLampRecharge") > 0.1f)
        {
            debouncing = false;
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
