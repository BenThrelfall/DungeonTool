using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behavour for tools that are always acitve
/// </summary>
public class PermTools : MonoBehaviour, IRequiresDependancy {

    [SerializeField]
    Camera mainCamera;

    //Camera Drag
    [SerializeField]
    Transform cameraTransform;
    Vector2 mouseStartPoint;

    //Camera Zoom
    [SerializeField]
    float zoomSensitivity;
    [SerializeField]
    float minZoom;
    [SerializeField]
    float maxZoom;

    IFrameRateLimiter limiter;

    private void Update() {
        CameraDragInputs();
        CameraZoomInputs();
    }

    private void CameraZoomInputs() {
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize - Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity, minZoom, maxZoom);
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
