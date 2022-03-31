using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionUpdateEventHandler : MonoBehaviour, IVisionUpdateEventHandler {
    

    public event Action VisionUpdate;

    public void InvokeVisionUpdate() {
        VisionUpdate?.Invoke();
    }
}
