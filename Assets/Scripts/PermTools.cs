using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behavour for tools that are always acitve
/// </summary>
public class PermTools : MonoBehaviour, IRequiresDependancy {

    //Camera Drag
    [SerializeField]
    Transform cameraTransform;

    [SerializeField]
    Camera mainCamera;

    Vector2 mouseStartPoint;

    IFrameRateLimiter limiter;

    private void Update() {
        CameraDragInputs();
    }

    private void CameraDragInputs() {

        if (Input.GetMouseButtonDown(2)) {
            mouseStartPoint = MousePos();
            limiter.StartActivity();
        }

        if (Input.GetMouseButton(2)) {
            var diff = MousePos() - mouseStartPoint;
            cameraTransform.Translate(-diff);
        }

        if (Input.GetMouseButtonUp(2)) {
            limiter.StopActivity();
        }

    }

    Vector2 MousePos() {
        return mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    public void SetUpDependancies(ServiceCollection serviceCollection) {
        limiter = serviceCollection.GetService<IFrameRateLimiter>();
    }
}
