using UnityEngine;
using System.Collections;

public class HeadsUpDisplayController : MonoBehaviour {

    public Texture2D noiseDisplay;
    public GameObject player;
    public Material noiseMat;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        if (Event.current.type.Equals(EventType.Repaint))
        {
            Color colPreviousGUIColor = GUI.color;
            float opacity = GameObject.Find("Player").GetComponent<MentalStability>().insanity;
            GUI.color = new Color(colPreviousGUIColor.r, colPreviousGUIColor.g, colPreviousGUIColor.b, opacity);

            int cols = Mathf.CeilToInt(Screen.width / noiseDisplay.width) + 1;
            int rows = Mathf.CeilToInt(Screen.height / noiseDisplay.height) + 1;
            int xoff = Random.Range(0, noiseDisplay.width);
            int yoff = Random.Range(0, noiseDisplay.height);

            for (int x = -1; x < cols; x++)
            {
                for (int y = -1; y < rows; y++)
                {
                    GUI.DrawTexture(new Rect(x * noiseDisplay.width + xoff, y * noiseDisplay.height + yoff, noiseDisplay.width, noiseDisplay.height), noiseDisplay);
                    //Graphics.DrawTexture(new Rect(x * noiseDisplay.width + xoff, y * noiseDisplay.height + yoff, noiseDisplay.width, noiseDisplay.height), noiseDisplay, new Rect(0, 0, 1, 1), 0, 0, 0, 0, GUI.color, noiseMat);
                }
            }
            Graphics.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), noiseDisplay, new Rect(0, 0, 1, 1), 0, 0, 0, 0, new Color(0, 0, 0, opacity*opacity), noiseMat);

            GUI.color = colPreviousGUIColor;
        }
    }
}
