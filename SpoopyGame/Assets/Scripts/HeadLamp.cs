using UnityEngine;
using System.Collections;

public class HeadLamp : MonoBehaviour {

    public AudioClip recharge, rechargeFull, clickOn, clickOff, clickDead;
    public float batteryLifeInSeconds = 60;
    public float numberOfShakesToCharge = 6;
    public float weakenTime = 5;

    public float currentBatteryLife;
    private bool isTurnedOn = false;
    private float maxIntensity;
    private float maxGlow;
    private Light glow;

    private float deltaAccel;
    private float tapRest;
    private float tapThreshold = 150;
    private float tapDelay = 0.1f;

    private float lastTimeWithCharge;
    public float delayForPromptingRecharge = 8;

    Debouncer.DebouncerResults headLampToggleCorrected, headLampChargeCorrected;
	void Start () 
	{
        lastTimeWithCharge = Time.time;
        currentBatteryLife = batteryLifeInSeconds;
        glow = transform.FindChild("Glow").GetComponent<Light>();
        maxIntensity = GetComponent<Light>().intensity;
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


        if (currentBatteryLife > 0)
        {
            lastTimeWithCharge = Time.time;
        }

        if (Time.time - lastTimeWithCharge > delayForPromptingRecharge)
        {
            HeadsUpDisplayController.Instance.DrawText("Press (X) Several Times to Recharge Your Headlamp", 0, 0.5f, Color.yellow, 0.05f);
            HeadsUpDisplayController.Instance.DrawText("Then Press (Y) to Turn Your Headlamp On", 0, 0.3f, Color.yellow, 0.05f);
        }

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
            GetComponent<Light>().enabled = Random.value <= weakenedPower;
            GetComponent<Light>().intensity = weakenedPower * maxIntensity;
            glow.intensity = weakenedPower * maxGlow;
        }
        else
        {
            GetComponent<Light>().enabled = false;
        }

        glow.enabled = GetComponent<Light>().enabled;

	}

    public void TurnOn()
    {
        if (!isTurnedOn)
        {
            if (SoundManagerController.Instance != null)
                SoundManagerController.Instance.PlaySoundAt((currentBatteryLife > 0) ? clickOn : clickDead, transform.position, "PlayerMadeSound");
            if (currentBatteryLife > 0)
                isTurnedOn = true;
        }
    }

    public void TurnOff()
    {
        if (isTurnedOn)
        {
            if (SoundManagerController.Instance != null)
                SoundManagerController.Instance.PlaySoundAt(clickOff, transform.position, "PlayerMadeSound");
            isTurnedOn = false;
        }
    }
}
