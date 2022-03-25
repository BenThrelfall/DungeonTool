using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAddTokenToBoardButton : MonoBehaviour, IRequiresDependancy {

    public string tokenHash;

    IObjectSpawner spawner;
    
    [SerializeField]
    bool dontAutoDependancies;

    private void Start() {
        if (dontAutoDependancies == false) SetUpDependancies(DependancyInjector.instance.Services);
    }

    public void OnClick() {
        spawner.SpawnObject(IObjectSpawner.SpawnType.token, tokenHash);
    }

    public void SetUpDependancies(ServiceCollection serviceCollection) {
        spawner = serviceCollection.GetService<IObjectSpawner>();
    }
}
