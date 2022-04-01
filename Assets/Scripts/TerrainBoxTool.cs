using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainBoxTool : MonoBehaviour, IRequiresDependancy {

    IObjectSpawner spawner;

    [SerializeField]
    Camera mainCamera;

    [SerializeField]
    GameObject boxIndicator;

    Vector2 startPos;
    Vector2 diag;

    const float minSize = 0.2f;

    [SerializeField]
    LayerMask terrainLayerMask;

    public void DoPlacement() {

        if (Input.GetMouseButtonDown(0)) {
            startPos = MousePos();
            boxIndicator.SetActive(true);
        }

        if (Input.GetMouseButton(0)) {
            diag = startPos - MousePos();

            boxIndicator.transform.position = startPos - diag * 0.5f;
            boxIndicator.transform.localScale = new Vector3(diag.x, diag.y, 1);
        }

        if (Input.GetMouseButtonUp(0)) {
            if (boxIndicator.transform.localScale.x * boxIndicator.transform.localScale.x > minSize) {
                SpawnBox(boxIndicator.transform.position, boxIndicator.transform.localScale);
            }
            boxIndicator.SetActive(false);
        }

        if (Input.GetMouseButtonDown(1)) {
            RemoveBox();
        }

    }

    private void RemoveBox() {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, 100f, terrainLayerMask);

        if (hit.collider is null) return;

        spawner.DespawnObject(hit.collider.gameObject);
    }

    void SpawnBox(Vector3 centre, Vector3 size) {
        spawner.SpawnObject(IObjectSpawner.SpawnType.terrainBox, "", centre, Quaternion.identity, size);
    }

    Vector2 MousePos() {
        return mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    public void SetUpDependancies(ServiceCollection serviceCollection) {
        spawner = serviceCollection.GetService<IObjectSpawner>();
    }
}
