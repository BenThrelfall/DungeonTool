using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Behavour that goes on each UI element that switches the active tool.
/// A specific tool to switch to is assigned to each behavour instance. 
/// </summary>
public class ToolSwitchingButton : MonoBehaviour, IRequiresDependancy {

    IToolManager toolManager;

    /// <summary>
    /// Text that will be highlighted when this instances <c>target</c> tool is active 
    /// </summary>
    [SerializeField]
    TextMeshProUGUI text;

    /// <summary>
    /// Tool that will be switched to
    /// </summary>
    [SerializeField]
    DungTool target;

    private void Start() {
        SetUpDependancies(DependancyInjector.instance.Services);
    }

    /// <summary>
    /// Switch the tool manager to this instances <c>target</c> tool
    /// </summary>
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

    /// <summary>
    /// Called by event on <c>ToolManager</c>
    /// Highlight the text if this instances <c>target</c> tool is active.
    /// Otherwise change the text colour back to normal
    /// </summary>
    /// <param name="tool">Tool that is now active</param>
    private void ToolManager_ToolChanged(DungTool tool) {
        if (tool == target) {
            text.color = Color.green;
        }
        else {
            text.color = Color.black;
        }
    }
}
