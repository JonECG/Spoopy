using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Insanity : MonoBehaviour
{
    public float percentPerSecond = 0.3f;
    private LightDetector lightDetect;
    // Use this for initialization
    void Start()
    {
        lightDetect = FindObjectOfType<LightDetector>();
    }

    // Update is called once per frame
    void Update()
    {
        //Get this object's position in the camera's viewpoint to test and see if it's onscreen.
        Vector3 campos = lightDetect.camera.WorldToViewportPoint(this.transform.position);
        //if all the values are positive and between 0 and 1, this object is on screen.

        float blinkCoverage = Mathf.Min(FindObjectOfType<Blinker>().BlinkLeftPercentage, FindObjectOfType<Blinker>().BlinkRightPercentage);
        bool checkIfOnScreen = (campos.x > 0.0f && campos.x < 1.0f && campos.y > 0.0f && campos.y < 1.0f - blinkCoverage && campos.z > 0.0f);

        if (checkIfOnScreen)
        {
            //Checking to see if it's behind an object
            RaycastHit tempHit;
            Physics.Raycast(lightDetect.transform.position, (this.transform.position - lightDetect.transform.position).normalized, out tempHit);

            //if we cast a ray to hit the object and the object hit is this object then we can do the sanity stuff
            if (tempHit.collider == this.collider)
            {
                Color seenPixel = lightDetect.capturedTex.GetPixel((int)(campos.x*lightDetect.capturedTex.width), (int)(campos.y*lightDetect.capturedTex.height));
                float avgColorSeenScaled = Mathf.Min( ( seenPixel.r + seenPixel.g + seenPixel.b ), 1 );
                FindObjectOfType<MentalStability>().insanity += percentPerSecond * Time.deltaTime * avgColorSeenScaled;
            }
        }
    }
}
