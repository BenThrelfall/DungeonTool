using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogTool : MonoBehaviour, IRequiresDependancy {

    IObjectSpawner objectSpawner;

    [SerializeField]
    Camera mainCamera;

    [SerializeField]
    GameObject fogIndicator;

    Vector2 startPos;
    Vector2 diag;

    const float minSize = 0.2f;

    [SerializeField]
    LayerMask fogLayerMask;

    public void DoPlacement() {

        if (Input.GetMouseButtonDown(0)) {
            startPos = MousePos();
            fogIndicator.SetActive(true);
        }

        if (Input.GetMouseButton(0)) {
            diag = startPos - MousePos();

            fogIndicator.transform.position = startPos - diag * 0.5f;
            fogIndicator.transform.localScale = new Vector3(diag.x, diag.y, 1);
        }

        if (Input.GetMouseButtonUp(0)) {
            if (fogIndicator.transform.localScale.x * fogIndicator.transform.localScale.x > minSize) {
                SpawnFog(fogIndicator.transform.position, fogIndicator.transform.localScale);
            }
            fogIndicator.SetActive(false);
        }

        if (Input.GetMouseButtonDown(1)) {
            RemoveFog();
        }

    }

    private void RemoveFog() {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, 100f, fogLayerMask);

        if (hit.collider is null) return;

        objectSpawner.DespawnObject(hit.collider.gameObject);
    }


    void SpawnFog(Vector3 centre, Vector3 size) {
        objectSpawner.SpawnObject(IObjectSpawner.SpawnType.fog, "", centre, Quaternion.identity, size);
    }

    Vector2 MousePos() {
        return mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    public void SetUpDependancies(ServiceCollection serviceCollection) {
        objectSpawner = serviceCollection.GetService<IObjectSpawner>();
    }
}
