using UnityEngine;
using System.Collections;

public class Hearing : SenseInterface {

    AudioSource[] heardInterests;
    int lastIndex;

    private void AudioSourceMade(AudioSource sounded)
    {
        if (sounded.tag == "AlertingSound" || sounded.tag == "PlayerMadeSound")
        {
            Debug.Log("Alerted");
            if (lastIndex < heardInterests.Length)
                heardInterests[lastIndex++] = sounded;
            else
                heardInterests[0] = sounded;
        }
    }

	void Start () 
	{
        heardInterests = new AudioSource[50];
        lastIndex = 0;
        SoundManagerController.Instance.SoundCreatedEvent += AudioSourceMade;
	}

    void OnDestroy()
    {
        SoundManagerController.Instance.SoundCreatedEvent -= AudioSourceMade;
    }

    public override Brain.SensedInfo Sense()
    {
        Brain.SensedInfo result = new Brain.SensedInfo();

        int addedCount = 0;

        for (int i = 0; i < lastIndex; i++)
        {
            if (heardInterests[i] == null)
            {
                heardInterests[i] = heardInterests[--lastIndex];
            }
            if (i < lastIndex && heardInterests[i] != null)
            {
                //float[] data = new float[1];
                //heardInterests[i].clip.GetData(data, heardInterests[i].timeSamples);
                //float level = data[0];
                float ratio = Mathf.Max(0, distance - (heardInterests[i].transform.position - transform.position).magnitude) / distance;
                float intensity = ratio * ratio;
                float sqrtRatio = Mathf.Sqrt(ratio);

                Debug.Log("Listening");
                result.CertaintyIsPlayer += ((heardInterests[i].tag == "PlayerMadeSound") ? 1 : 0) * sqrtRatio;
                result.CertaintyOfDirection += sqrtRatio;
                result.CertaintyOfDistance += sqrtRatio;
                result.AlertingFactor += sqrtRatio;
                result.SensedDirection += (heardInterests[i].transform.position - transform.position).normalized;
                result.SensedDistance += (heardInterests[i].transform.position - transform.position).magnitude;
                addedCount++;
            }
        }

        if (lastIndex > 0)
        {
            result.CertaintyIsPlayer /= addedCount;
            result.CertaintyOfDirection /= addedCount;
            result.CertaintyOfDistance /= addedCount;
            result.AlertingFactor /= addedCount;
            result.SensedDirection /= addedCount;
            result.SensedDistance /= addedCount;
            result.SensedDirection.Normalize();
        }

        return result;
    }
}
