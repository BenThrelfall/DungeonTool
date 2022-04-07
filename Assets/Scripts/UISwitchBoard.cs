using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISwitchBoard : MonoBehaviour, IRequiresDependancy {

    public int boardTarget;

    IBoardManager boardManager;

    public void SetUpDependancies(ServiceCollection serviceCollection) {
        boardManager = serviceCollection.GetService<IBoardManager>();
    }

    void Start() {
        SetUpDependancies(DependancyInjector.instance.Services);
    }

    public void OnClick() {
        boardManager.SwitchToBoard(boardTarget);
    }

}

