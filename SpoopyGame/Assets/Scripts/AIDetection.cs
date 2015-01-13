using UnityEngine;
using System.Collections;

public class AIDetection : MonoBehaviour
{

    private bool recentlyDetected = false;
    private Vector3 playerDirection;
    public float viewAngle = 90;
    public float viewDistance = 8;
    public float patrolSpeed = 2.0f;
    public float chaseSpeed = 3.0f;
    public float exploreCoordX=0;
    public float exploreCoordZ=0;
    public float exploreDistance = 20;
    private Vector2 exploreNode;
    // Use this for initialization

    void Start()
    {
        exploreCoordX = transform.position.x;
        exploreCoordZ = transform.position.z;

        exploreNode.x = Random.Range(exploreCoordX-exploreDistance, exploreCoordX+exploreDistance);
        exploreNode.y = Random.Range(exploreCoordZ-exploreDistance, exploreCoordZ+exploreDistance);

        while((Physics.Raycast(transform.position, new Vector3(exploreNode.x, transform.position.y, exploreNode.y)-transform.position, Vector3.Distance(transform.position, new Vector3(exploreNode.x, transform.position.y, exploreNode.y)))))
        {
            exploreNode.x = Random.Range(exploreCoordX - exploreDistance, exploreCoordX + exploreDistance);
            exploreNode.y = Random.Range(exploreCoordZ - exploreDistance, exploreCoordZ + exploreDistance);
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject playerObject = GameObject.Find("Player");

        if (Vector3.Distance(playerObject.transform.position, transform.position)<viewDistance)
        {
            Vector3 player = playerObject.rigidbody.position;
            playerDirection = player - transform.position;
            float angle = Vector3.Angle(playerDirection, transform.forward);
            if (angle < (viewAngle * 0.5f))
            {
                RaycastHit hitOne;

                if (Physics.Raycast(transform.position + transform.up, playerDirection, out hitOne, viewDistance))
                {
                    if (hitOne.collider.gameObject == playerObject)
                    {
                       transform.LookAt(new Vector3(player.x, transform.position.y, player.z));
                        if( recentlyDetected == false )
                            GetComponent<SoundStatePlayer>().PlaySoundFrom("FoundPlayer");
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
                exploreNode.x = Random.Range(exploreCoordX - exploreDistance, exploreCoordX + exploreDistance);
                exploreNode.y = Random.Range(exploreCoordZ - exploreDistance, exploreCoordZ + exploreDistance);

                while((Physics.Raycast(transform.position, new Vector3(exploreNode.x, transform.position.y, exploreNode.y)-transform.position, Vector3.Distance(transform.position, new Vector3(exploreNode.x, transform.position.y, exploreNode.y)))))
                {
                    exploreNode.x = Random.Range(exploreCoordX - exploreDistance, exploreCoordX + exploreDistance);
                    exploreNode.y = Random.Range(exploreCoordZ - exploreDistance, exploreCoordZ + exploreDistance);
                }
            }
            Debug.DrawLine(transform.position, new Vector3(exploreNode.x, transform.position.y, exploreNode.y));
            transform.LookAt(new Vector3(exploreNode.x, transform.position.y, exploreNode.y));
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            transform.Translate((transform.forward.normalized*patrolSpeed)*Time.deltaTime, Space.World);
            renderer.material.color = Color.white;
            GetComponent<SoundStatePlayer>().SetState("Patrol");
        }
        else if (recentlyDetected)
        {
            exploreCoordX=playerObject.rigidbody.position.x;
            exploreCoordZ=playerObject.rigidbody.position.z;

            transform.Translate((transform.forward.normalized*chaseSpeed)*Time.deltaTime, Space.World);
            renderer.material.color = Color.red;
            GetComponent<SoundStatePlayer>().SetState("Chase");
        }
    }

    void OnGUI()
    {
        if(Input.GetKey(KeyCode.H))
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 200, 250), "Velocity X: " + rigidbody.angularVelocity.x + " Velocity Y: " + rigidbody.angularVelocity.y + " Velocity Z: " + rigidbody.angularVelocity.z);
    }
}