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
    void Start()
    {
        SphereCollider myCollider = transform.GetComponent<SphereCollider>();
        myCollider.radius = viewDistance;

        exploreNode.x = Random.Range(exploreCoordX, exploreCoordX + exploreDistance);
        exploreNode.y = Random.Range(exploreCoordZ, exploreCoordZ + exploreDistance);

        float angle = Vector3.Angle(new Vector3(exploreNode.x, transform.position.y, exploreNode.y), transform.forward);

        transform.RotateAround(transform.position, transform.up, angle);
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
                    }
                }
            }
            else
            {
                recentlyDetected = false;
            }
        }

        if (!recentlyDetected)
        {
            if (Mathf.Abs(exploreNode.x-transform.position.x) < 1.0f && Mathf.Abs(exploreNode.y-transform.position.z) < 1.0f)
            {
                exploreNode.x = Random.Range(exploreCoordX, exploreCoordX + exploreDistance);
                exploreNode.y = Random.Range(exploreCoordZ, exploreCoordZ + exploreDistance);

                transform.LookAt(new Vector3(exploreNode.x, transform.position.y, exploreNode.y));
            }
            Vector3 pd = (new Vector3(exploreNode.x, transform.position.y, exploreNode.y))-(transform.position);
            pd = pd.normalized * speed;
            transform.Translate(pd.x, 0.0f, pd.z);
        }
        else if (recentlyDetected)
        {
            exploreCoordX=playerObject.rigidbody.position.x;
            exploreCoordZ=playerObject.rigidbody.position.z;
            Vector3 pd = (transform.position) - (playerObject.rigidbody.position);
            pd = pd.normalized * speed;
            transform.Translate(pd.x, 0.0f, pd.z);
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
}