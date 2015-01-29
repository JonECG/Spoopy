using UnityEngine;
using System.Collections;

public class DisableTriggerable : Triggerable {

    public GameObject targetGameObject;
    public MonoBehaviour targetComponent;

    public bool enableVsDisable = true;


    public override void Triggered(string id)
    {
        if (targetGameObject != null)
            targetGameObject.SetActive(enableVsDisable);
        if (targetComponent != null)
            targetComponent.enabled = enableVsDisable;

        if (targetComponent == null && targetGameObject == null)
            gameObject.SetActive(enableVsDisable);
    }
}
