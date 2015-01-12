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
                renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, Mathf.Clamp( renderer.material.color.a - 2 * Time.deltaTime, 0 , 1 ));
            }
            else
            {
                fading = false;
            }
        }
    }

    public override void Triggered(string id)
    {
        fading = true;
    }
}
