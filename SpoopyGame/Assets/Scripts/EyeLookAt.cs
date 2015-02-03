using UnityEngine;
using System.Collections;

public class EyeLookAt : MonoBehaviour
{
    private float randomY;
    private float randomX;
    private float randomZ;
    bool isUp;
    bool isWeave;
    bool isZMove;

    void Start ()
    {
        isUp = randomBoolean();
        isWeave = randomBoolean();
        isZMove = randomBoolean();
        randomX = Random.Range(-0.2f, 0.2f);
        randomY = Random.Range(-0.2f, 0.2f);
        randomZ=Random.Range(-0.2f,0.2f);
        transform.position = new Vector3(transform.position.x, transform.position.y + randomY, transform.position.z);
        transform.position = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z);
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + randomZ);
    }

    bool randomBoolean()
    {
        if (Random.value >= 0.5)
            return true;

        return false;
    }

	// Update is called once per frame
	void Update ()
    {
        GameObject player = GameObject.Find("LitCamera");
        transform.LookAt(player.transform.position-new Vector3(0.0f,0.5f,0.0f));

        if (randomY >= 0.2)
        {
            isUp = false;
        }
        else if (randomY <= -0.2)
        {
            isUp = true;
        }

        if (randomX >= 0.2)
        {
            isWeave = false;
        }
        else if (randomX <= -0.2)
        {
            isWeave = true;
        }

        if (randomZ >= 0.2f)
        {
            isZMove = false;
        }
        else if (randomZ <= -0.2f)
        {
            isZMove = true;
        }

        if (isUp)
        {
            transform.position += new Vector3(0, 0.001f, 0);
            randomY += 0.001f;
        }
        else
        {
            transform.position -= new Vector3(0, 0.001f, 0);
            randomY -= 0.001f;
        }

        if (isWeave)
        {
            transform.position += new Vector3(0.001f, 0, 0);
            randomX += 0.001f;
        }
        else
        {
            transform.position -= new Vector3(0.001f, 0, 0);
            randomX -= 0.001f;
        }

        if (isZMove)
        {
            transform.position += new Vector3(0, 0, 0.001f);
            randomZ += 0.001f;
        }
        else
        {
            transform.position -= new Vector3(0, 0, 0.001f);
            randomZ -= 0.001f;
        }
	}
}
