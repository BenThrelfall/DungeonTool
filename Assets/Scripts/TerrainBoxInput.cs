using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainBoxInput : MonoBehaviour {

    [SerializeField]
    TerrainBoxTool tool;

    private void Update() {
        tool.DoPlacement();
    }

}
