using UnityEngine;
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
}
