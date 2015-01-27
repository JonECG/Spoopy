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
	void Awake () {
        sprintStamina = 1;
        head = GameObject.Find("Head").transform;

        if (Application.isEditor)
        {
            GameObject.Destroy(GameObject.Find("OVRCameraRig"));
            anchor = GameObject.Find("Head").transform;
        }
        else
        {
            GameObject.Destroy(GameObject.Find("NormalCamera"));
            anchor = GameObject.Find("CenterEyeAnchor").transform;
        }

        GameObject.Find("LitCamera").transform.SetParent( anchor, false );
        GameObject.Find("Headlamp").transform.SetParent(anchor, false);
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


        float headAngle = Mathf.Atan2(forwardReference.z, forwardReference.x);
        float playerAngle = Mathf.Atan2(transform.forward.z, transform.forward.x);
        float diffAngle = Mathf.Deg2Rad * (Mathf.Repeat((Mathf.Repeat((playerAngle - headAngle) * Mathf.Rad2Deg, 360)) + 540, 360) - 180);
        float normDiffAngle = Mathf.Sign( diffAngle ) * Mathf.Pow( diffAngle / Mathf.PI, 2 );
        normDiffAngle *= 7;

        float moveMultiplier = normDiffAngle / Mathf.Sqrt(1 + normDiffAngle * normDiffAngle) + 1;
        moveMultiplier = Mathf.Pow(moveMultiplier, 2);
        moveMultiplier /= 2;

        float invMoveMultiplier = -normDiffAngle / Mathf.Sqrt(1 + normDiffAngle * normDiffAngle) + 1;
        invMoveMultiplier = Mathf.Pow(invMoveMultiplier, 2);
        invMoveMultiplier /= 2;

        //HeadsUpDisplayController.Instance.DrawText(normDiffAngle.ToString(), 0, 0.2f, Color.green);
        //HeadsUpDisplayController.Instance.DrawText(moveMultiplier.ToString(), 0, 0.3f, Color.yellow);

        if (GameObject.Find("LeftEyeAnchor") != null)
        {
            transform.Rotate(new Vector3(0, 1, 0), longinalStick * mouseSensitivity * ( longinalStick > 0 ? moveMultiplier : invMoveMultiplier ));
        }
        else
        {
            transform.Rotate(new Vector3(0, 1, 0), (lateral + longinalStick ) * mouseSensitivity);
            head.Rotate(new Vector3(1, 0, 0), -(longinal) * mouseSensitivity);
        }
	}
}
