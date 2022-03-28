using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ToolSwitchingButton : MonoBehaviour, IRequiresDependancy {

    IToolManager toolManager;

    [SerializeField]
    TextMeshProUGUI text;

    [SerializeField]
    DungTool target;

    public void Clicked() {
        toolManager.SwitchToTool(target);
    }

    public void SetUpDependancies(ServiceCollection serviceCollection) {
        toolManager = serviceCollection.GetService<IToolManager>();
        toolManager.ToolChanged += ToolManager_ToolChanged;
    }

    private void OnDisable() {
        toolManager.ToolChanged -= ToolManager_ToolChanged;
    }

    private void ToolManager_ToolChanged(DungTool tool) {
        if (tool == target) {
            text.color = Color.green;
        }
        else {
            text.color = Color.black;
        }
    }
}
