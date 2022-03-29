using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Limits the framerate of the game based if there is activity or not.
/// This is mostly for WebGL if hardware acceleration is not enabled in the 
/// browser. In such cases, without limiting the framerate performance is extremely poor.
/// </summary>
public interface IFrameRateLimiter {

    /// <summary>
    /// Indicate that an activity that calls for a higher frame rate has started.
    /// <c>StopActivity</c> should be called when the activity is finished.
    /// </summary>
    void StartActivity();

    /// <summary>
    /// Indicate that an activity that calls for a higher frame rate has stopped.
    /// </summary>
    void StopActivity();

}
