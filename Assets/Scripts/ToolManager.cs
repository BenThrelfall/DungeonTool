using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implementation of <c>IToolManager</c>
/// Uses GameObjects as the tool instances 
/// and calles set active on the GameObjects 
/// to enable or disable them.
/// </summary>
public class ToolManager : NetworkBehaviour, IToolManager {

    Dictionary<DungTool, GameObject> tools = new Dictionary<DungTool, GameObject>();
    DungTool activeTool;
    [SerializeField]bool toolsEnabled;

    [SerializeField]
    GameObject selectTool;

    [SerializeField]
    GameObject pointerTool;

    [SerializeField]
    GameObject rulerTool;

    [SerializeField]
    GameObject circleRuler;

    [SerializeField]
    GameObject fogTool;

    [SerializeField]
    GameObject terrainBox;

    [SerializeField]
    GameObject terrainLine;

    [SerializeField]
    GameObject addLight;

    [SerializeField]
    GameObject permTools;

    [SerializeField]
    List<GameObject> toolUIObjects;

    public event Action<DungTool> ToolChanged;

    public override void OnStartClient() {
        base.OnStartClient();

        toolsEnabled = true;
        SwitchToTool(DungTool.Select);
    }

    private void Start() {
        tools.Add(DungTool.Ruler, rulerTool);
        tools.Add(DungTool.Select, selectTool);
        tools.Add(DungTool.Pointer, pointerTool);
        tools.Add(DungTool.CircleRuler, circleRuler);
        tools.Add(DungTool.Fog, fogTool);
        tools.Add(DungTool.TerrainBox, terrainBox);
        tools.Add(DungTool.TerrainLine, terrainLine);
        tools.Add(DungTool.AddLight, addLight);
    }

    public void SwitchToTool(DungTool tool) {

        if (activeTool == tool) return;

        DisableAllToolUIObjects();
        SetActiveTool(tool);
        if (!toolsEnabled) return;
        EnableToolGameObject();
    }

    private void EnableToolGameObject() {

        foreach (var item in tools) {
            if (item.Key == activeTool) {
                item.Value.SetActive(true);
            }
            else {
                item.Value.SetActive(false);
            }
        }
    }

    void DisableAllToolUIObjects() {
        foreach (var obj in toolUIObjects) {
            obj.SetActive(false);
        }
    }

    private void SetActiveTool(DungTool tool) {
        activeTool = tool;
        ToolChanged?.Invoke(tool);
    }

    public void PauseAllTools() {
        if (!gameObject.activeSelf) return;
        toolsEnabled = false;
        tools[activeTool].SetActive(false);
        permTools.SetActive(false);
    }

    public void ResumeAllTools() {
        if (!gameObject.activeSelf) return;
        toolsEnabled = true;
        EnableToolGameObject();
        permTools.SetActive(true);
    }
}
