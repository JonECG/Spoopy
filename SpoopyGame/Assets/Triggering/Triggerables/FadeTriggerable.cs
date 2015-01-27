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
            if ((renderer.material.color.a > 0 && !FadeInVsFadeOut) || (renderer.material.color.a < 1 && FadeInVsFadeOut))
            {
                renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, Mathf.Clamp(renderer.material.color.a + ( ( FadeInVsFadeOut ? 1 : -1 ) / TimeForTotalFade * Time.deltaTime ), 0, 1));
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
