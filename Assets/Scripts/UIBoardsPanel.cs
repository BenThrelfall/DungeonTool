using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBoardsPanel : MonoBehaviour, IRequiresDependancy {

    IBoardManager boardManager;

    [SerializeField]
    GameObject buttonArea;

    [SerializeField]
    GameObject switchBoardButtonPrefab;

    List<GameObject> buttons = new List<GameObject>();

    public void SetUpDependancies(ServiceCollection serviceCollection) {
        boardManager = serviceCollection.GetService<IBoardManager>();
        boardManager.boardsUpdated += BoardManager_boardsUpdated;
        boardManager.RequestBoardUpdate();
    }

    private void BoardManager_boardsUpdated(int boards) {
        foreach (var item in buttons) {
            Destroy(item);
        }

        buttons.Clear();

        for (int i = 0; i < boards; i++) {
            var button = Instantiate(switchBoardButtonPrefab, buttonArea.transform);
            var comp = button.GetComponent<UISwitchBoard>();
            comp.boardTarget = i;
            buttons.Add(button);
        }

    }

    void Start() {
        SetUpDependancies(DependancyInjector.instance.Services);
    }

}
