using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChangeMapButton : MonoBehaviour, IRequiresDependancy {

    public string boardHash;

    IMapUpdater mapUpader;

    [SerializeField]
    bool dontAutoDependancies;

    private void Start() {
        if (dontAutoDependancies == false) SetUpDependancies(DependancyInjector.instance.Services);
    }

    public void OnClick() {
        mapUpader.UpdateMap(boardHash);
    }

    public void SetUpDependancies(ServiceCollection serviceCollection) {
        mapUpader = serviceCollection.GetService<IMapUpdater>();
    }
}
