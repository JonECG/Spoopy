using UnityEngine;
using System.Collections;

public class AIDetection : MonoBehaviour
{

    private bool withinRange = false;
    private bool recentlyDetected = false;
    private Vector3 playerDirection;
    public float viewAngle = 90;
    public float viewDistance = 8;
    public float speed = 0.4f;
    public float exploreCoordX=0;
    public float exploreCoordZ=0;
    public float exploreDistance = 20;
    private Vector2 exploreNode;
    // Use this for initialization

    private Vector3 testVec;
    void Start()
    {
        SphereCollider myCollider = transform.GetComponent<SphereCollider>();
        myCollider.radius = viewDistance;

        exploreNode.x = Random.Range(exploreCoordX, exploreCoordX + exploreDistance);
        exploreNode.y = Random.Range(exploreCoordZ, exploreCoordZ + exploreDistance);

        while ((Physics.Raycast(transform.position + new Vector3(0,1,0), new Vector3(exploreNode.x, transform.position.y, exploreNode.y))))
        {
            exploreNode.x = Random.Range(exploreCoordX, exploreCoordX + exploreDistance);
            exploreNode.y = Random.Range(exploreCoordZ, exploreCoordZ + exploreDistance);
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject playerObject = GameObject.Find("Player");

        if (withinRange)
        {
            Vector3 player = playerObject.rigidbody.position;
            playerDirection = player - transform.position;
            float angle = Vector3.Angle(playerDirection, transform.forward);
            if (angle < (viewAngle * 0.5f))
            {
                RaycastHit hitOne;

                SphereCollider myCollider = transform.GetComponent<SphereCollider>();

                if (Physics.Raycast(transform.position + transform.up, playerDirection.normalized, out hitOne, myCollider.radius))
                {
                    if (hitOne.collider.gameObject == playerObject)
                    {
                       transform.LookAt(new Vector3(player.x, transform.position.y, player.z));
                        recentlyDetected = true;
                    }
                    else
                        recentlyDetected = false;
                }
                else
                    recentlyDetected = false;
            }
            else
            {
                recentlyDetected = false;
            }
        }

        if (!recentlyDetected)
        {
            if (((exploreNode.x-transform.position.x < 1.0f && exploreNode.x-transform.position.x > -1.0f) && (exploreNode.y-transform.position.z < 1.0f && exploreNode.y-transform.position.z>-1.0f)))
            {
                exploreNode.x = Random.Range(exploreCoordX, exploreCoordX + exploreDistance);
                exploreNode.y = Random.Range(exploreCoordZ, exploreCoordZ + exploreDistance);

                while ((Physics.Raycast(transform.position + transform.up, new Vector3(exploreNode.x, transform.position.y, exploreNode.y))))
                {
                    exploreNode.x = Random.Range(exploreCoordX, exploreCoordX + exploreDistance);
                    exploreNode.y = Random.Range(exploreCoordZ, exploreCoordZ + exploreDistance);
                }
            }
            Debug.DrawLine(transform.position, new Vector3(exploreNode.x, transform.position.y, exploreNode.y));
            transform.LookAt(new Vector3(exploreNode.x, transform.position.y, exploreNode.y));
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            transform.Translate(transform.forward.normalized*speed, Space.World);
        }
        else if (recentlyDetected)
        {
            exploreCoordX=playerObject.rigidbody.position.x;
            exploreCoordZ=playerObject.rigidbody.position.z;

            transform.Translate(transform.forward.normalized*speed, Space.World);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Player")
        {
            withinRange = true;
        }
        else
            withinRange = false;
    }

    void OnGUI()
    {
        if(Input.GetKey(KeyCode.H))
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 200, 250), "Velocity X: " + rigidbody.angularVelocity.x + " Velocity Y: " + rigidbody.angularVelocity.y + " Velocity Z: " + rigidbody.angularVelocity.z);
    }
}