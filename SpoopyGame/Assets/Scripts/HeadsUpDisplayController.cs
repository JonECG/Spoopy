using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class HeadsUpDisplayController : MonoBehaviour {

    public Texture2D noiseDisplay;
    public GameObject player;
    public Material noiseMat;

    public Material multiply, additive;
    public DarkVision darkVision;

    public Camera[] targetCameras;

    private Texture2D whiteBlit;

    private Vector3 delta;

    private Image additivePlane, multiplyPlane, noisePlane, blinkPlane, blinkFadePlane;

    //private Camera target;

    public static HeadsUpDisplayController Instance { get; protected set; }

    static List<HeadsUpDisplayController> clones = new List<HeadsUpDisplayController>();
    static HeadsUpDisplayController master;

    private int currentText = 0;
    private List<Text> textsInUse = new List<Text>();

    public GameObject textReference;

    int offsetOfFlags = 20;

    bool hasOculus;

    void OnGUI()
    {
        if (Instance == this)
        {
            for (int i = 0; i < currentText; i++)
            {
                textsInUse[i].enabled = false;
            }
            currentText = 0;
        }
    }

    public void DrawText(string message, float x, float y, Color color, float size = 0.1f, float time = 0)
    {
        if (Instance == this)
        {

            if (time <= 0)
            {
                if (currentText >= textsInUse.Count)
                {
                    GameObject dupe = (GameObject)GameObject.Instantiate(textReference);
                    dupe.transform.SetParent(textReference.transform.parent.transform, false);
                    textsInUse.Add(dupe.GetComponent<Text>());
                }

                Text t = textsInUse[currentText++];

                t.enabled = true;
                t.text = message;
                t.color = color;
                RectTransform crt = t.transform.parent.GetComponent<Canvas>().GetComponent<RectTransform>();
                float mult = 1;
                if (hasOculus)
                    mult = 0.7f;
                t.fontSize = (int)(crt.sizeDelta.x * size * mult);
                t.rectTransform.localPosition = (0.85f * mult) * new Vector3(x * crt.sizeDelta.x, y * crt.sizeDelta.y, 0);
                //t.rectTransform.position = new Vector3(0, 0, 0);
            }
            else
            {
                StartCoroutine(PersistedText(message, x, y, color, size, time));
            }
        }

    }

    IEnumerator PersistedText(string message, float x, float y, Color color, float size, float time)
    {
        float endTime = Time.time + time;
        while (Time.time < endTime)
        {
            //Debug.Log("Drawing persisted text " + ( endTime - Time.time ));

            DrawText(message, x, y, color, size);
            yield return null;
        }
    }


    void SetLayerRecursively( GameObject obj, int newLayer )
    {
        obj.layer = newLayer;
   
        foreach( Transform child in obj.transform )
        {
            SetLayerRecursively( child.gameObject, newLayer );
        }
    }

	// Use this for initialization
	void Start () {
        hasOculus = (GameObject.Find("OVRCameraRig") != null);
        //GameObject one = transform.FindChild("NoiseLayer").gameObject;
        //GameObject two = one.transform.FindChild("NoisePlane").gameObject;
        //Image imageFound = two.GetComponent<Image>();
        additivePlane = transform.FindChild("AdditivePlane").GetComponent<Image>();
        multiplyPlane = transform.FindChild("MultiplyPlane").GetComponent<Image>();
        noisePlane = transform.FindChild("NoisePlane").GetComponent<Image>();
        blinkPlane = transform.FindChild("BlinkPlane").GetComponent<Image>();
        blinkFadePlane = transform.FindChild("BlinkFadePlane").GetComponent<Image>();
        if (master == null)
        {
            master = this;
            Instance = this;
            clones.Clear();
            
            int cullAll = 0;
            for (int i = 0; i < targetCameras.Length; i++)
            {
                cullAll |= 1 << (i + offsetOfFlags);
            }
            for (int i = 0; i < targetCameras.Length; i++)
            {
                GameObject dupe = (GameObject)Instantiate(gameObject);
                dupe.name = "HUD for " + targetCameras[i].name;
                //dupe.GetComponent<HeadsUpDisplayController>().target = targetCameras[i];
                clones.Add(dupe.GetComponent<HeadsUpDisplayController>());
                SetLayerRecursively(dupe, i + offsetOfFlags);
                //dupe.layer = i + offsetOfFlags;
                targetCameras[i].cullingMask = ( ~cullAll ) | ( 1 << ( i + offsetOfFlags ) );
                dupe.transform.SetParent( targetCameras[i].transform, false );
            }
            GetComponent<Canvas>().enabled = false;
        }
	}

    void Update()
    {
        float insane = player.GetComponent<MentalStability>().insanity;
        float health = player.GetComponent<HealthyLiving>().health;

        int xoff = Random.Range(0, noiseDisplay.width);
        int yoff = Random.Range(0, noiseDisplay.height);


        noisePlane.color = new Color(1, 1, 1, insane);
        float invSqrInsane = 1 - (insane * insane);
        Color fade = new Color(invSqrInsane, invSqrInsane * health, invSqrInsane * health, 1);

        Vector3 rat = new Vector3(darkVision.lightDetector.averageColorAsVec.x / darkVision.adjustedLight.x,
            darkVision.lightDetector.averageColorAsVec.y / darkVision.adjustedLight.y,
            darkVision.lightDetector.averageColorAsVec.z / darkVision.adjustedLight.z);
        delta = (delta * 0 + (darkVision.lightDetector.averageColorAsVec - darkVision.adjustedLight)) / 1;

        multiplyPlane.material.color = new Color(Mathf.Min(1 + delta.x * 3, 1) * fade.r, Mathf.Min(1 + delta.y * 3, 1) * fade.g, Mathf.Min(1 + delta.z * 3, 1) * fade.b, 1);
        additivePlane.material.color = new Color(Mathf.Max(delta.x, 0), Mathf.Max(delta.y, 0), Mathf.Max(delta.z, 0), 1);
        //delta = (delta + rat) / 2;

        //multiplyPlane.material.color = new Color(Mathf.Min(delta.x, 1) * fade.r, Mathf.Min(delta.y, 1) * fade.g, Mathf.Min(delta.z, 1) * fade.b, 1);
        //additivePlane.material.color = new Color(Mathf.Max(delta.x - 1, 0), Mathf.Max(delta.y - 1, 0), Mathf.Max(delta.z-1, 0), 1);
        noisePlane.rectTransform.localPosition = new Vector3(-xoff, -yoff, noisePlane.rectTransform.localPosition.z);
        noisePlane.rectTransform.sizeDelta = noisePlane.transform.parent.GetComponent<RectTransform>().sizeDelta + new Vector2(xoff, yoff) * 2;
        multiplyPlane.rectTransform.sizeDelta = noisePlane.transform.parent.GetComponent<RectTransform>().sizeDelta * 1.1f;
        blinkPlane.rectTransform.sizeDelta = noisePlane.transform.parent.GetComponent<RectTransform>().sizeDelta * 1.1f;
        blinkFadePlane.rectTransform.sizeDelta = noisePlane.transform.parent.GetComponent<RectTransform>().sizeDelta * 1.1f;
        additivePlane.rectTransform.sizeDelta = noisePlane.transform.parent.GetComponent<RectTransform>().sizeDelta * 1.1f;

        float blinkYOff = noisePlane.transform.parent.GetComponent<RectTransform>().sizeDelta.y;
        float usedBlink, usedFadeBlink;
        if( transform.parent.name == "RightEyeAnchor" )
        {
            usedBlink = Mathf.Min(player.GetComponent<Blinker>().BlinkLeftPercentage, player.GetComponent<Blinker>().BlinkRightPercentage);
            usedFadeBlink = player.GetComponent<Blinker>().BlinkRightPercentage;
        }
        else
        if (transform.parent.name == "LeftEyeAnchor")
        {
            usedBlink = Mathf.Min( player.GetComponent<Blinker>().BlinkLeftPercentage, player.GetComponent<Blinker>().BlinkRightPercentage );
            usedFadeBlink = player.GetComponent<Blinker>().BlinkLeftPercentage;
        }
        else
        {
            usedBlink = Mathf.Min(player.GetComponent<Blinker>().BlinkLeftPercentage, player.GetComponent<Blinker>().BlinkRightPercentage);
            usedFadeBlink = Mathf.Max(player.GetComponent<Blinker>().BlinkLeftPercentage, player.GetComponent<Blinker>().BlinkRightPercentage);
        }
        blinkPlane.rectTransform.localPosition = new Vector3(0, ((1.0f - usedBlink) * blinkYOff * 2), blinkPlane.rectTransform.localPosition.z);
        blinkFadePlane.rectTransform.localPosition = new Vector3(0, ((1.0f - usedFadeBlink) * blinkYOff * 2), blinkFadePlane.rectTransform.localPosition.z);
    }

    //void OnGUI()
    //{
    //    if (Event.current.type.Equals(EventType.Repaint))
    //    {
    //        Color colPreviousGUIColor = GUI.color;
    //        float opacity = 0.5f;// GameObject.Find("Player").GetComponent<MentalStability>().insanity;
    //        GUI.color = new Color(colPreviousGUIColor.r, colPreviousGUIColor.g, colPreviousGUIColor.b, opacity);

    //        int cols = Mathf.CeilToInt(Screen.width / noiseDisplay.width) + 1;
    //        int rows = Mathf.CeilToInt(Screen.height / noiseDisplay.height) + 1;
    //        int xoff = Random.Range(0, noiseDisplay.width);
    //        int yoff = Random.Range(0, noiseDisplay.height);

    //        for (int x = -1; x < cols; x++)
    //        {
    //            for (int y = -1; y < rows; y++)
    //            {
    //                GUI.DrawTexture(new Rect(x * noiseDisplay.width + xoff, y * noiseDisplay.height + yoff, noiseDisplay.width, noiseDisplay.height), noiseDisplay);
    //                //Graphics.DrawTexture(new Rect(x * noiseDisplay.width + xoff, y * noiseDisplay.height + yoff, noiseDisplay.width, noiseDisplay.height), noiseDisplay, new Rect(0, 0, 1, 1), 0, 0, 0, 0, GUI.color, noiseMat);
    //            }
    //        }
    //        Graphics.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), noiseDisplay, new Rect(0, 0, 1, 1), 0, 0, 0, 0, new Color(0, 0, 0, opacity*opacity), multiply);

    //        delta = (delta + darkVision.lightDetector.averageColorAsVec - darkVision.adjustedLight) / 2;

    //        multiply.color = new Color(Mathf.Min(1 + delta.x, 1), Mathf.Min(1 + delta.y, 1), Mathf.Min(1 + delta.z, 1), 1);
    //        additive.color = new Color(Mathf.Max(delta.x, 0), Mathf.Max(delta.y, 0), Mathf.Max(delta.z, 0), 1);
            
    //        Graphics.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), whiteBlit, additive);
    //        Graphics.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), whiteBlit, multiply);

    //        GUI.color = colPreviousGUIColor;
    //    }
    //}
}
