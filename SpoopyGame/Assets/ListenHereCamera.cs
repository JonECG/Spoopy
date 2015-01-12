using UnityEngine;
using System.Collections;

public class ListenHereCamera : MonoBehaviour {

    public delegate void OnPreRenderEventHandler(Camera current);
    public static event OnPreRenderEventHandler OnPreRenderEvent;

    void LateUpdate()
    {
        if (OnPreRenderEvent != null)
            OnPreRenderEvent( camera );
    }
}
