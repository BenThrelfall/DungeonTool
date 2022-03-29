using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Disables all tools when the mouse is over regions of the screen determined by the event system
/// </summary>
public class ToolDisabler : MonoBehaviour, IRequiresDependancy {

    IToolManager toolManager;

    bool disabled = false;

    public void SetUpDependancies(ServiceCollection serviceCollection) {
        toolManager = serviceCollection.GetService<IToolManager>();
    }

    private void Update() {
        
        if (EventSystem.current.IsPointerOverGameObject()) {
            if (!disabled) {
                toolManager.PauseAllTools();
                disabled = true;
            }
        }
        else if(disabled) {
            toolManager.ResumeAllTools();
            disabled = false;
        }

    }

}
