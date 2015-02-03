using UnityEngine;
using System.Collections;

public class Patrolling : ActingInterface
{
    public Vector3 patrolHere=new Vector3(0.0f,0.0f,0.0f);
    Vector3 goHere=new Vector3(0.0f,0.0f,0.0f);
    public float patrolDistance = 5;
    public float patrolSpeed = 0.4f;

    void Start()
    {
        patrolHere = transform.position;
    }

    public override void Act(Brain.Perception perceived, Brain.Motivation motivation)
    {

        Vector3 diff = new Vector3( goHere.x - transform.position.x, 0, goHere.z - transform.position.z );
        if (goHere.sqrMagnitude < 0.1 || diff.sqrMagnitude < 0.3f)
        {
            goHere.x = patrolHere.x + Random.Range(-patrolDistance, patrolDistance);
            goHere.z = patrolHere.z + Random.Range(-patrolDistance, patrolDistance);
            goHere.y = transform.position.y;

            RaycastHit info;
            if (Physics.Raycast(transform.position, (goHere - transform.position).normalized, out info, (goHere - transform.position).magnitude + 2, 1 << LayerMask.NameToLayer("Map")) && info.distance < (goHere - transform.position).magnitude + 2)
            {
                goHere = transform.position + (goHere - transform.position).normalized * Mathf.Max(0, info.distance - 2);
            }
        }
        transform.LookAt(new Vector3(goHere.x, transform.position.y, goHere.z));
        transform.Translate((transform.forward.normalized * patrolSpeed) * Time.deltaTime, Space.World);
    }
}
