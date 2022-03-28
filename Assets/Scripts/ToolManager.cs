using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolManager : NetworkBehaviour, IToolManager {

    Dictionary<DungTool, GameObject> tools = new Dictionary<DungTool, GameObject>();
    GameObject activeTool;
    GameObject lastTool;

    [SerializeField]
    GameObject selectTool;

    [SerializeField]
    GameObject pointerTool;

    [SerializeField]
    GameObject rulerTool;

    public event Action<DungTool> ToolChanged;

    public override void OnStartClient() {
        DisableAllTools();
    }

    private void Start() {
        tools.Add(DungTool.Ruler, rulerTool);
        tools.Add(DungTool.Select, selectTool);
        tools.Add(DungTool.Pointer, pointerTool);
    }

    public void DisableAllTools() {

        lastTool = activeTool;
        activeTool = null;

        foreach (var tool in tools.Values) {
            tool.SetActive(false);
        }
    }

    public void EnableLastTool() {

        var temp = activeTool;
        activeTool = lastTool;
        lastTool = temp;

    }

    public void SwitchToTool(DungTool tool) {
        lastTool = activeTool;

        foreach (var item in tools) {
            if(item.Key == tool) {
                activeTool = item.Value;
                item.Value.SetActive(true);
            }
            else {
                item.Value.SetActive(false);
            }
        }

        ToolChanged?.Invoke(tool);

    }
}
