using UnityEngine;
using System.Collections;

public class DEBUGVIEW : MonoBehaviour {

	void Update () 
	{
        int row = 0;
        float vOff = 0.5f;
        float vGap = 0.04f;
        float hOff = -0.3f;
        float hGap = 0.04f;
        float textSize = 0.02f;

        Brain b = GetComponent<Brain>();

        HeadsUpDisplayController.Instance.DrawText("BRAIN", hOff, vOff - vGap * (row++), Color.red, textSize);
            HeadsUpDisplayController.Instance.DrawText("Alertness: " + b.Alertness, hOff + hGap, vOff - vGap * (row++), Color.white, textSize);
            HeadsUpDisplayController.Instance.DrawText("CertaintyIsPlayer: " + b.CertaintyIsPlayer, hOff + hGap, vOff - vGap * (row++), Color.white, textSize);
            HeadsUpDisplayController.Instance.DrawText("CertaintyOfDistance: " + b.CertaintyOfDistance, hOff + hGap, vOff - vGap * (row++), Color.white, textSize);
            HeadsUpDisplayController.Instance.DrawText("PerceivedDistance: " + b.PerceivedDistance, hOff + hGap, vOff - vGap * (row++), Color.white, textSize);
            HeadsUpDisplayController.Instance.DrawText("CertaintyOfDirection: " + b.CertaintyOfDirection, hOff + hGap, vOff - vGap * (row++), Color.white, textSize);
            HeadsUpDisplayController.Instance.DrawText("PerceivedDirection: <" + b.PerceivedDirection.x + "," + b.PerceivedDirection.y + "," + b.PerceivedDirection.z + ">", hOff + hGap, vOff - vGap * (row++), Color.white, textSize);
            HeadsUpDisplayController.Instance.DrawText("PerceivedPosition: <" + b.PerceivedWorldPosition.x + "," + b.PerceivedWorldPosition.y + "," + b.PerceivedWorldPosition.z + ">", hOff + hGap, vOff - vGap * (row++), Color.white, textSize);

        HeadsUpDisplayController.Instance.DrawText("SENSES", hOff, vOff - vGap * (row++), Color.red, textSize);
        SenseInterface[] senses = GetComponents<SenseInterface>();
        foreach (SenseInterface sense in senses)
        {
            HeadsUpDisplayController.Instance.DrawText(sense.GetType().Name, hOff + hGap, vOff - vGap * (row++), Color.yellow, textSize);
        }

        HeadsUpDisplayController.Instance.DrawText("THOUGHTS", hOff, vOff - vGap * (row++), Color.red, textSize);
        ThoughtInterface[] thoughts = GetComponents<ThoughtInterface>();
        foreach (ThoughtInterface thought in thoughts)
        {
            HeadsUpDisplayController.Instance.DrawText(thought.GetType().Name, hOff + hGap, vOff - vGap * (row++), Color.yellow, textSize);
        }

        HeadsUpDisplayController.Instance.DrawText("ACTING", hOff, vOff - vGap * (row++), Color.red, textSize);
        ActingInterface[] actions = GetComponents<ActingInterface>();
        foreach (ActingInterface action in actions)
        {
            HeadsUpDisplayController.Instance.DrawText(action.GetType().Name, hOff + hGap, vOff - vGap * (row++), Color.yellow, textSize);
        }
	}
}
