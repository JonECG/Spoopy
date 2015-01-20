using UnityEngine;
using System.Collections;

public class Patrolling : ActingInterface
{
    public Vector3 patrolHere=new Vector3(0.0f,0.0f,0.0f);
    Vector3 goHere=new Vector3(0.0f,0.0f,0.0f);
    public float patrolDistance = 5;
    public float patrolSpeed = 0.4f;

    public override void Act(Brain.Perception perceived, Brain.Motivation motivation)
    {
        patrolHere=perceived.PerceivedWorldPosition;
        if ((goHere.x == 0.0f && goHere.y == 0.0f && goHere.z == 0.0f) || ((goHere.x - transform.position.x < 1.0f && goHere.x - transform.position.x > -1.0f) && (goHere.z - transform.position.z < 1.0f && goHere.z - transform.position.z > -1.0f)))
        {
            goHere.x = Random.Range(patrolHere.x - patrolDistance, patrolHere.x + patrolDistance);
            goHere.z = Random.Range(patrolHere.z - patrolDistance, patrolHere.z + patrolDistance);

            while ((Physics.Raycast(transform.position, new Vector3(goHere.x, transform.position.y, goHere.z) - transform.position, Vector3.Distance(new Vector3(goHere.x, transform.position.y, goHere.z), transform.position))))
            {
                goHere.x = Random.Range(patrolHere.x - patrolDistance, patrolHere.x + patrolDistance);
                goHere.z = Random.Range(patrolHere.z - patrolDistance, patrolHere.z + patrolDistance);
            }
        }
        transform.LookAt(new Vector3(goHere.x, transform.position.y, goHere.z));
        transform.Translate((transform.forward.normalized * patrolSpeed) * Time.deltaTime, Space.World);
        //GetComponent<SoundStatePlayer>().SetState("Patrol");
    }
}
