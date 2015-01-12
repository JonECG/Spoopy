using UnityEngine;
using System.Collections;

public class LightDetector : MonoBehaviour {

    public Color averageColor;
    public Vector3 averageColorAsVec;
	void Start () 
	{
        RenderTexture r = new RenderTexture(16, 16, 16);
        r.Create();
        camera.targetTexture = r;
	}
	
	void OnPostRender () 
	{
        averageColorAsVec = new Vector3();

        Texture2D texture = new Texture2D(camera.targetTexture.width,camera.targetTexture.height);
        Rect rect = new Rect( 0, 0, texture.width, texture.height );
        texture.ReadPixels(rect, 0, 0);
        //testTexture.Apply();
        

        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                Color pxl = texture.GetPixel(x,y);
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
