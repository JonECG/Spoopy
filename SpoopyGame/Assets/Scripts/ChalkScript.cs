using UnityEngine;
using System.Collections;

public class ChalkScript : MonoBehaviour 
{
    private Camera theCamera;
    public GameObject chalkquad;
    private float distance = 4.0f;
    private Vector3 lastPos;
    private float threshold = 0.002f;

    Debouncer.DebouncerResults chalkCorrected;
	// Use this for initialization
	void Start () 
    {
        theCamera = GameObject.Find("LitCamera").camera;
	}
	
	// Update is called once per frame
	void Update () 
    {
        chalkCorrected = Debouncer.Debounce("Chalk", chalkCorrected);
        if (chalkCorrected.IsDown())
        {
            placeChalk();
        }
	}

    void placeChalk()
    {
        RaycastHit hit;
        Physics.Raycast(theCamera.transform.position, theCamera.transform.forward, out hit, distance, 1 << LayerMask.NameToLayer("Map"));
        if (hit.collider)
        {
            Vector3 tempPos = hit.point + hit.normal * Random.Range(0.01f, 0.02f);
            if ((tempPos - lastPos).sqrMagnitude > threshold)
            {
                GameObject t = (GameObject)Instantiate(chalkquad, tempPos, Quaternion.identity);
                t.transform.LookAt(t.transform.position + (hit.normal * -1));
                t.transform.SetParent(hit.collider.gameObject.transform.root, true);
                lastPos = tempPos;
            }
        }
    }
}
