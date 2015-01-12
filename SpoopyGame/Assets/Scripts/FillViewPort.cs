using UnityEngine;
using System.Collections;

public class FillViewPort : MonoBehaviour {

    public float distanceAway = 0.1f;

	void Start() 
	{
        ListenHereCamera.OnPreRenderEvent += FillView;
	}

    void OnDestroy()
    {
        //Debug.Log("Destroyed");
        ListenHereCamera.OnPreRenderEvent -= FillView;
    }

    void FillView( Camera cam )
    {
        if ( ( (1 << gameObject.layer) & cam.cullingMask) != 0 && gameObject.name != "Hud" )
        {
            //Debug.Log(gameObject.name + " is adjusting to " + cam.gameObject.name);
            Vector3 v3ViewPort = new Vector3(0, 0, distanceAway);
            Vector3 v3BottomLeft = cam.ViewportToWorldPoint(v3ViewPort);
            v3ViewPort.Set(1, 1, distanceAway);
            Vector3 v3TopRight = cam.ViewportToWorldPoint(v3ViewPort);

            float width = Mathf.Abs(Vector3.Dot(cam.transform.right, v3TopRight) - Vector3.Dot(cam.transform.right, v3BottomLeft));
            float height = Mathf.Abs(Vector3.Dot(cam.transform.up, v3TopRight) - Vector3.Dot(cam.transform.up, v3BottomLeft));

            RectTransform rect = GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(cam.targetTexture.width, cam.targetTexture.height);
            rect.localScale = new Vector3(width / (cam.targetTexture.width * cam.rect.width), height / (cam.targetTexture.height * cam.rect.height), 1);
            rect.position = cam.transform.position + cam.transform.forward * distanceAway;
            rect.rotation = cam.transform.rotation;

            string[] layerNames = { "AllOne", "AllTwo", "AllThree" };
            for (int i = 0; i < transform.childCount; i++)
            {
                //Debug.Log(transform.GetChild(i).name);
                //childTransforms[i].localScale = rect.localScale;// new Vector3(1 / rect.localScale.x, 1 / rect.localScale.y, 1 / rect.localScale.z);
                transform.GetChild(i).GetComponent<RectTransform>().sizeDelta = new Vector2(10000, 10000);// rect.sizeDelta; 
                Vector3 dsl = transform.GetChild(i).GetComponent<RectTransform>().localPosition;
                dsl.z = -i * 0.001f;
                transform.GetChild(i).GetComponent<RectTransform>().localPosition = dsl;
                // new Vector3(0, 0, i * 0.001f);
                transform.GetChild(i).GetComponent<Canvas>().overrideSorting = true;
                //transform.GetChild(i).GetComponent<Canvas>().sortingLayerName = layerNames[i];// = new Vector2(10000, 10000);
                //transform.GetChild(i).GetComponent<Canvas>().sortingOrder = i;
            }
        }
    }
}
