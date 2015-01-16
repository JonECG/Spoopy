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
        Color seenPixel;
        bool isSeen = lightDetect.CanSee(gameObject, out seenPixel );
        
        if( isSeen )
        {
            float avgColorSeenScaled = Mathf.Min( ( seenPixel.r + seenPixel.g + seenPixel.b )*2, 1 );
            FindObjectOfType<MentalStability>().insanity += percentPerSecond * Time.deltaTime * avgColorSeenScaled;
        }
    }
}
