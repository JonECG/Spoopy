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
    private float maxGlow;
    private Light glow;

    private float deltaAccel;
    private float tapRest;
    private float tapThreshold = 150;
    private float tapDelay = 0.1f;

    Debouncer.DebouncerResults headLampToggleCorrected, headLampChargeCorrected;
	void Start () 
	{
        isTurnedOn = false;
        currentBatteryLife = batteryLifeInSeconds;
        glow = transform.FindChild("Glow").light;
        maxIntensity = light.intensity;
        maxGlow = glow.intensity;
	}
	
	void Update () 
	{
        bool tapped = false;
        //if( !Application.isEditor )
        {
            Ovr.Vector3f ang = OVRManager.capiHmd.GetTrackingState().HeadPose.AngularAcceleration;
            deltaAccel = Mathf.Abs(ang.x) + Mathf.Abs(ang.y) + Mathf.Abs(ang.z);

            tapRest -= Time.deltaTime;

            if (deltaAccel > tapThreshold && tapRest <= 0)
            {
                Debug.Log("Tap");
                deltaAccel = 0;
                tapRest = tapDelay;
                tapped = true;
            }
        }

        headLampToggleCorrected = Debouncer.Debounce("HeadLampToggle", headLampToggleCorrected);
        headLampChargeCorrected = Debouncer.Debounce("HeadLampRecharge", headLampChargeCorrected);

        if (headLampToggleCorrected.IsPressed())
        {
            if( SoundManagerController.Instance != null )
                SoundManagerController.Instance.PlaySoundAt(isTurnedOn ? clickOff : (currentBatteryLife > 0) ? clickOn : clickDead, transform.position, "PlayerMadeSound");
            if (currentBatteryLife > 0)
                isTurnedOn = !isTurnedOn;
        }

        if (headLampChargeCorrected.IsPressed() || tapped)
        {
            currentBatteryLife = Mathf.Min(batteryLifeInSeconds, currentBatteryLife + batteryLifeInSeconds / numberOfShakesToCharge);
            if (SoundManagerController.Instance != null)
                SoundManagerController.Instance.PlaySoundAt((currentBatteryLife == batteryLifeInSeconds) ? rechargeFull : recharge, transform.position, "PlayerMadeSound");
        }

        if (isTurnedOn)
        {
            currentBatteryLife -= Time.deltaTime;

            if (currentBatteryLife <= 0)
            {
                currentBatteryLife = 0;
                isTurnedOn = false;
                if (SoundManagerController.Instance != null)
                    SoundManagerController.Instance.PlaySoundAt(clickOff, transform.position, "PlayerMadeSound");
            }
        }

        if (isTurnedOn)
        {
            float weakenedPower = (currentBatteryLife > weakenTime) ? 1 : currentBatteryLife / weakenTime;
            light.enabled = Random.value <= weakenedPower;
            light.intensity = weakenedPower * maxIntensity;
            glow.intensity = weakenedPower * maxGlow;
        }
        else
        {
            light.enabled = false;
        }

        glow.enabled = light.enabled;

	}
}
