using UnityEngine;
using System.Collections;

public class DisappearTriggerable : Triggerable {

    private bool fading = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (fading)
        {
            if (renderer.material.color.a > 0)
            {
                renderer.material.SetColor("Main Color", new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, renderer.material.color.a - 100 * Time.deltaTime));

            }
            else
            {
                fading = false;
            }
        }
    }


    void Triggered()
    {
        fading = true;
    }
}
