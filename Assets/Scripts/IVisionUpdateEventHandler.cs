using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVisionUpdateEventHandler {

    event Action VisionUpdate;

    void InvokeVisionUpdate(float delay);

}
