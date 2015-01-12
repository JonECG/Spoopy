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

    private Image additivePlane, multiplyPlane, noisePlane, blinkPlane;

    //private Camera target;

    static List<HeadsUpDisplayController> clones = new List<HeadsUpDisplayController>();
    static HeadsUpDisplayController master;

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

        //GameObject one = transform.FindChild("NoiseLayer").gameObject;
        //GameObject two = one.transform.FindChild("NoisePlane").gameObject;
        //Image imageFound = two.GetComponent<Image>();
        additivePlane = transform.FindChild("AdditivePlane").GetComponent<Image>();
        multiplyPlane = transform.FindChild("MultiplyPlane").GetComponent<Image>();
        noisePlane = transform.FindChild("NoisePlane").GetComponent<Image>();
        blinkPlane = transform.FindChild("BlinkPlane").GetComponent<Image>();
        if (master == null)
        {
            master = this;
            clones.Clear();
            int offsetOfFlags = 2;
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
                dupe.transform.parent = targetCameras[i].transform;
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

        delta = (delta + darkVision.lightDetector.averageColorAsVec - darkVision.adjustedLight) / 2;

        multiplyPlane.material.color = new Color(Mathf.Min(1 + delta.x, 1) * fade.r, Mathf.Min(1 + delta.y, 1) * fade.g, Mathf.Min(1 + delta.z, 1) * fade.b, 1);
        additivePlane.material.color = new Color(Mathf.Max(delta.x, 0), Mathf.Max(delta.y, 0), Mathf.Max(delta.z, 0), 1);
        noisePlane.rectTransform.localPosition = new Vector3(-xoff, -yoff, noisePlane.rectTransform.localPosition.z);
        noisePlane.rectTransform.sizeDelta = noisePlane.transform.parent.GetComponent<RectTransform>().sizeDelta + new Vector2(xoff, yoff) * 2;
        multiplyPlane.rectTransform.sizeDelta = noisePlane.transform.parent.GetComponent<RectTransform>().sizeDelta * 1.1f;
        blinkPlane.rectTransform.sizeDelta = noisePlane.transform.parent.GetComponent<RectTransform>().sizeDelta * 1.1f;
        additivePlane.rectTransform.sizeDelta = noisePlane.transform.parent.GetComponent<RectTransform>().sizeDelta * 1.1f;

        float blinkYOff = noisePlane.transform.parent.GetComponent<RectTransform>().sizeDelta.y;
        float usedBlink = (transform.parent.name == "RightEyeAnchor") ? player.GetComponent<Blinker>().BlinkRightPercentage : player.GetComponent<Blinker>().BlinkLeftPercentage;
        blinkPlane.rectTransform.localPosition = new Vector3(0, ((1.0f - usedBlink) * blinkYOff * 2), blinkPlane.rectTransform.localPosition.z);
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
