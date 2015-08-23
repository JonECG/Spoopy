using UnityEngine;
using System.Collections;

public class FadeTriggerable : Triggerable {

    private bool fading = false;
    public float TimeForTotalFade = 2;
    public bool FadeInVsFadeOut = true;
    public Triggerable OnFadeEnd;
    private string heldString;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (fading)
        {
            if ((GetComponent<Renderer>().material.color.a > 0 && !FadeInVsFadeOut) || (GetComponent<Renderer>().material.color.a < 1 && FadeInVsFadeOut))
            {
                GetComponent<Renderer>().material.color = new Color(GetComponent<Renderer>().material.color.r, GetComponent<Renderer>().material.color.g, GetComponent<Renderer>().material.color.b, Mathf.Clamp(GetComponent<Renderer>().material.color.a + ( ( FadeInVsFadeOut ? 1 : -1 ) / TimeForTotalFade * Time.deltaTime ), 0, 1));
            }
            else
            {
                fading = false;
                if (OnFadeEnd != null)
                    OnFadeEnd.Triggered(heldString);
            }
        }
    }

    public override void Triggered(string id)
    {
        heldString = id;
        fading = true;
    }
}
