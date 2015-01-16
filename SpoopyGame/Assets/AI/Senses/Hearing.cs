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
                heardInterests[lastIndex] = sounded;
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
        for (int i = 0; i < lastIndex; i++)
        {
            if (heardInterests[i] == null)
            {
                heardInterests[i] = heardInterests[lastIndex--];
            }

            //float[] data = new float[1];
            //heardInterests[i].clip.GetData(data, heardInterests[i].timeSamples);
            //float level = data[0];
            float ratio = Mathf.Max(0, distance - (heardInterests[i].transform.position - transform.position).magnitude) / distance;
            float intensity = ratio*ratio;
            float sqrtRatio = Mathf.Sqrt( ratio );

            result.CertaintyIsPlayer += ((heardInterests[i].tag == "PlayerMadeSound") ? 1 : 0) * sqrtRatio;
            result.CertaintyOfDirection += sqrtRatio;
            result.CertaintyOfDistance += sqrtRatio;
            result.AlertingFactor += intensity;
            result.SensedDirection += (heardInterests[i].transform.position - transform.position).normalized;
            result.SensedDistance += (heardInterests[i].transform.position - transform.position).magnitude;
        }

        result.CertaintyIsPlayer /= heardInterests.Length;
        result.CertaintyOfDirection /= heardInterests.Length;
        result.CertaintyOfDistance /= heardInterests.Length;
        result.AlertingFactor /= heardInterests.Length;
        result.SensedDirection /= heardInterests.Length;
        result.SensedDistance /= heardInterests.Length;
        result.SensedDirection.Normalize();

        return result;
    }
}
