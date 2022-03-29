using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implementation of <c>IFrameRateLimiter</c>
/// Switches between a set constant low fps if there are no 
/// activities ongoing to an unlimited high fps if there is 
/// at least one activity ongoing
/// </summary>
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
