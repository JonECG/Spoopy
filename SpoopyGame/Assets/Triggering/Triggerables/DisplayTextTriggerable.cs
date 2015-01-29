using UnityEngine;
using System.Collections;

public class DisplayTextTriggerable : Triggerable {

    public string prompt;
    public Vector2 position;
    public float time;
    public Color color;
    public float size = 0.1f;

    public override void Triggered(string id)
    {
        HeadsUpDisplayController.Instance.DrawText(prompt, position.x, position.y, color, size, time);
    }
}
