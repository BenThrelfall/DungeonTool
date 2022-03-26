using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateLimiter : MonoBehaviour, IFrameRateLimiter {

    int activityOccuring = 0;
    const int inActiveTarget = 16;

    private void Start() {
        if (activityOccuring == 0) Application.targetFrameRate = inActiveTarget;
    }

    public void StartActivity() {
        activityOccuring++;

        if (activityOccuring == 1) Application.targetFrameRate = -1;
    }

    public void StopActivity() {
        activityOccuring--;

        if (activityOccuring == 0) Application.targetFrameRate = inActiveTarget;
        if (activityOccuring < 0) throw new System.Exception("Invalid activity number");
    }

}
