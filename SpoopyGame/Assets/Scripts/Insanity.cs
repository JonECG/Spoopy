using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Visibility : MonoBehaviour
{
    public Camera playerViewCamera;
    private float sanity = 0.0f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Get this object's position in the camera's viewpoint to test and see if it's onscreen.
        Vector3 campos = playerViewCamera.WorldToViewportPoint(this.transform.position);
        //if all the values are positive and between 0 and 1, this object is on screen.
        bool checkIfOnScreen = (campos.x > 0.0f && campos.x < 1.0f && campos.y > 0.0f && campos.y < 1.0f && campos.z > 0.0f);

        if (checkIfOnScreen)
        {
            //Checking to see if it's behind an object
            RaycastHit tempHit;
            Physics.Raycast(playerViewCamera.transform.position, (this.transform.position - playerViewCamera.transform.position).normalized, out tempHit);

            //if we cast a ray to hit the object and the object hit is this object then we can do the sanity stuff
            if (tempHit.collider == this.collider)
            {
                if (sanity < 1.0f)
                {
                    sanity += 0.005f;
                }
                else
                {
                    sanity = 1.0f;
                }
            }
        }
    }
}
