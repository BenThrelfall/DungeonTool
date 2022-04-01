using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogInput : MonoBehaviour {

    [SerializeField]
    FogTool tool;

    void Update() {
        tool.DoPlacement();
    }
}
