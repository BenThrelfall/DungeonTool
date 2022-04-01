using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionUpdateEventHandler : MonoBehaviour, IVisionUpdateEventHandler {
    

    public event Action VisionUpdate;

    public void InvokeVisionUpdate(float delay) {
        StartCoroutine(CallAfterDelay(delay));
    }

    IEnumerator CallAfterDelay(float delay) {
        yield return new WaitForSecondsRealtime(delay);
        VisionUpdate?.Invoke();
    }
}
