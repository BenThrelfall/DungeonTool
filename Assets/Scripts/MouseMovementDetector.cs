using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovementDetector : MonoBehaviour, IRequiresDependancy {

    IFrameRateLimiter frameRateLimiter;

    bool active = false;

    const float speedCutoff = 50f;

    Vector3 lastPosition = Vector3.zero;

    public void SetUpDependancies(ServiceCollection serviceCollection) {
        frameRateLimiter = serviceCollection.GetService<IFrameRateLimiter>();
    }

    private void Update() {
        var currentPosition = Input.mousePosition;

        if (((currentPosition - lastPosition).sqrMagnitude / Time.deltaTime) > speedCutoff && !active) {
            frameRateLimiter.StartActivity();
            active = true;
        }
        else if (active) {
            frameRateLimiter.StopActivity();
            active = false;
        }

        lastPosition = currentPosition;
    }

}
