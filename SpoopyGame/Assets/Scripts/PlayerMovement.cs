using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {


    Transform anchor;
    Transform head;
    int moveSpeed = 3;
    float mouseSensitivity = 2.0f;

    public float sprintRecovery = 6;
    public float sprintDuration = 5;
    public float sprintMultiplier = 1.6f;
    float sprintStamina;
    Debouncer.DebouncerResults sprintCorrected;

	// Use this for initialization
	void Start () {
        sprintStamina = 1;
        head = GameObject.Find("Head").transform;

        if (!Application.isEditor)
        {
            GameObject.Destroy(GameObject.Find("OVRCameraRig"));
            anchor = GameObject.Find("Head").transform;
        }
        else
        {
            GameObject.Destroy(GameObject.Find("NormalCamera"));
            anchor = GameObject.Find("CenterEyeAnchor").transform;
        }

        GameObject.Find("LitCamera").transform.parent = anchor;
        GameObject.Find("Headlamp").transform.parent = anchor;
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 rightReference = anchor.rotation * new Vector3(1, 0, 0);
        Vector3 upReference = anchor.rotation * new Vector3(0, 1, 0);
        Vector3 forwardReference = anchor.rotation * new Vector3(0, 0, 1);

        sprintCorrected = Debouncer.Debounce("Sprint", sprintCorrected);
        if (sprintCorrected.IsDown())
            sprintStamina -= Time.deltaTime / sprintDuration;
        else
            sprintStamina += Time.deltaTime / sprintRecovery;
        sprintStamina = Mathf.Clamp(sprintStamina, 0, 1);

        transform.position += new Vector3(forwardReference.x, 0, forwardReference.z).normalized * (sprintCorrected.IsDown() && sprintStamina > 0 ? sprintMultiplier : 1) * Time.deltaTime * moveSpeed * Input.GetAxis("Vertical");
        transform.position += new Vector3(rightReference.x, 0, rightReference.z).normalized * (sprintCorrected.IsDown() && sprintStamina > 0 ? sprintMultiplier : 1) * Time.deltaTime * moveSpeed * Input.GetAxis("Horizontal");


        float lateral = Input.GetAxis("Mouse X");
        float longinal = Input.GetAxis("Mouse Y");
        float longinalStick = Input.GetAxis("TurningX");

        transform.Rotate(new Vector3(0, 1, 0), ( lateral + longinalStick) * mouseSensitivity );
        if( GameObject.Find( "LeftEyeAnchor" ) == null )
            head.Rotate(new Vector3(1, 0, 0), -(longinal) * mouseSensitivity);

        HeadsUpDisplayController.Instance.DrawText("12345678987654321", 0,0, Color.blue, 0.1f);
        HeadsUpDisplayController.Instance.DrawText("Look at me!", 0, 0.5f, Color.blue, 0.1f);
	}
}
