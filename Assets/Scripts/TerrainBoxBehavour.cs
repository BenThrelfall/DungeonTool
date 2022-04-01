using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainBoxBehavour : MonoBehaviour, IRequiresDependancy {

    IVisionUpdateEventHandler visionUpdateEvent;

    const float delay = 0.2f;

    private void Awake() {
        SetUpDependancies(DependancyInjector.instance.Services);
    }

    private void OnEnable() {
        visionUpdateEvent.InvokeVisionUpdate(delay);
    }

    private void OnDisable() {
        visionUpdateEvent.InvokeVisionUpdate(delay);
    }

    public void SetUpDependancies(ServiceCollection serviceCollection) {
        visionUpdateEvent = serviceCollection.GetService<IVisionUpdateEventHandler>();
    }
}
