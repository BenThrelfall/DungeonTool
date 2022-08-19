using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Checks for hotkey inputs and changes the currenlty selected tool appropriatly
/// </summary>
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
        if (Input.GetKeyDown(KeyCode.C)) {
            toolManager.SwitchToTool(DungTool.CircleRuler);
        }
        if (Input.GetKeyDown(KeyCode.F)) {
            toolManager.SwitchToTool(DungTool.Fog);
        }
        if (Input.GetKeyDown(KeyCode.B)) {
            toolManager.SwitchToTool(DungTool.TerrainBox);
        }
        if (Input.GetKeyDown(KeyCode.L)) {
            toolManager.SwitchToTool(DungTool.TerrainLine);
        }

    }

    public void SetUpDependancies(ServiceCollection serviceCollection) {
        toolManager = serviceCollection.GetService<IToolManager>();
    }
}
