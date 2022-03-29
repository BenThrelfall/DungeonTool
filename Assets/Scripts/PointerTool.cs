using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns a pointer tool that can be used to indicate things on the board
/// </summary>
public class PointerTool : NetworkBehaviour, IRequiresDependancy {

    [SerializeField]
    GameObject pointerPrefab;

    [SerializeField]
    Camera mainCamera;

    IFrameRateLimiter rateLimiter;

    private void Start() {
        if (mainCamera == null) mainCamera = Camera.main;
        SetUpDependancies(DependancyInjector.instance.Services);
    }

    public void PlacePointerTool() {
        if (Input.GetMouseButtonDown(0)) {
            Vector2 startPoint = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);
            CmdSpawnPointer(startPoint);
        }
    }

    [Command(requiresAuthority = false)]
    void CmdSpawnPointer(Vector2 startPoint, NetworkConnectionToClient conn = null) {
        GameObject pointer = Instantiate(pointerPrefab, startPoint, Quaternion.identity);
        NetworkServer.Spawn(pointer);
        TargetPointerSpawned(conn, startPoint, pointer);
    }

    [TargetRpc]
    private void TargetPointerSpawned(NetworkConnection conn, Vector2 startPoint, GameObject pointer) {
        StartCoroutine(MovePointerWithMouse(pointer));
    }

    private IEnumerator MovePointerWithMouse(GameObject pointer) {
        rateLimiter.StartActivity();
        Vector2 currentPos;

        while (Input.GetMouseButton(0)) {
            currentPos = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);

            pointer.transform.position = currentPos;
            CmdUpdatePointer(currentPos, pointer);

            yield return null;
        }

        CmdDestroyPointer(pointer);
        rateLimiter.StopActivity();
    }

    [Command(requiresAuthority = false)]
    private void CmdUpdatePointer(Vector2 position, GameObject pointer) {
        pointer.transform.position = position;
    }

    [Command(requiresAuthority = false)]
    private void CmdDestroyPointer(GameObject pointer) {
        NetworkServer.Destroy(pointer);
    }

    public void SetUpDependancies(ServiceCollection serviceCollection) {
        rateLimiter = serviceCollection.GetService<IFrameRateLimiter>();
    }
}
