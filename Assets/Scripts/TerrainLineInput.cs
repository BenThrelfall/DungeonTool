using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainLineInput : MonoBehaviour {

    [SerializeField]
    TerrainLineTool tool;

    private void Update() {
        tool.DoPlacement();
    }

}