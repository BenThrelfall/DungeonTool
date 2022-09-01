using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddLightTool : MonoBehaviour, IRequiresDependancy {

    [SerializeField]
    Camera mainCamera;

    IObjectSpawner objectSpawner;

    public void DoAddLight() {
        if (Input.GetMouseButtonDown(0)) {
            objectSpawner.SpawnObject(IObjectSpawner.SpawnType.light, "", MousePos());
        }
    }

    public void SetUpDependancies(ServiceCollection serviceCollection) {
        objectSpawner = serviceCollection.GetService<IObjectSpawner>();
    }

    Vector2 MousePos() {
        return mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }
}
