using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAddBoardButton : MonoBehaviour, IRequiresDependancy {

    IBoardManager boardManager;

    public void SetUpDependancies(ServiceCollection serviceCollection) {
        boardManager = serviceCollection.GetService<IBoardManager>();
    }

    void Start() {
        SetUpDependancies(DependancyInjector.instance.Services);
    }

    public void OnClick() {
        boardManager.CreateNewBoard();
    }

}
