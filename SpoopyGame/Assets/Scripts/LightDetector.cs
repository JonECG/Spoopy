﻿using UnityEngine;
using System.Collections;

public class LightDetector : MonoBehaviour {

    public Color averageColor;
    public Vector3 averageColorAsVec;
    public Texture2D capturedTex;

	void Start () 
	{
        RenderTexture r = new RenderTexture(16, 16, 16);
        r.Create();
        camera.targetTexture = r;
	}
	
	void OnPostRender () 
	{
        averageColorAsVec = new Vector3();

        capturedTex = new Texture2D(camera.targetTexture.width, camera.targetTexture.height);
        Rect rect = new Rect(0, 0, capturedTex.width, capturedTex.height);
        capturedTex.ReadPixels(rect, 0, 0);
        //testTexture.Apply();


        for (int x = 0; x < capturedTex.width; x++)
        {
            for (int y = 0; y < capturedTex.height; y++)
            {
                Color pxl = capturedTex.GetPixel(x, y);
                averageColorAsVec += new Vector3(pxl.r, pxl.g, pxl.b);
            }
        }

        averageColorAsVec /= camera.targetTexture.width * camera.targetTexture.height;

        averageColor = new Color(averageColorAsVec.x, averageColorAsVec.y, averageColorAsVec.z);
	}

    public Color GetColor()
    {
        return averageColor;
    }

    public bool CanSee( GameObject go, out Color seenColor )
    {
        seenColor = new Color(0, 0, 0);
        bool result = false;
        //Get this object's position in the camera's viewpoint to test and see if it's onscreen.
        Vector3 campos = camera.WorldToViewportPoint(go.transform.position);
        //if all the values are positive and between 0 and 1, this object is on screen.

        float blinkCoverage = Mathf.Min(FindObjectOfType<Blinker>().BlinkLeftPercentage, FindObjectOfType<Blinker>().BlinkRightPercentage);
        bool checkIfOnScreen = (campos.x > 0.0f && campos.x < 1.0f && campos.y > 0.0f && campos.y < 1.0f - blinkCoverage && campos.z > 0.0f);

        if (checkIfOnScreen)
        {
            //Checking to see if it's behind an object
            RaycastHit tempHit;
            Physics.Raycast(transform.position, (go.transform.position - transform.position).normalized, out tempHit);

            //if we cast a ray to hit the object and the object hit is this object then we can do the sanity stuff
            if (tempHit.collider == go.collider)
            {
                seenColor = capturedTex.GetPixel((int)(campos.x * capturedTex.width), (int)(campos.y * capturedTex.height));
                result = true;
            }
        }

        return result;
    }
}
