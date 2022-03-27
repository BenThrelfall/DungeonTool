using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotKeys : MonoBehaviour, IRequiresDependancy {

    IToolManager toolManager;

    private void Update() {

        if (Input.GetKeyDown(KeyCode.P)) {
            toolManager.SwitchToTool(DungTool.Pointer);
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            toolManager.SwitchToTool(DungTool.Select);
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            toolManager.SwitchToTool(DungTool.Ruler);
        }

    }

    public void SetUpDependancies(ServiceCollection serviceCollection) {
        toolManager = serviceCollection.GetService<IToolManager>();
    }
}
