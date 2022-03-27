using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RulerInput : MonoBehaviour {

    [SerializeField]
    RulerTool tool;
    void Update() {
        tool.PlaceRulerTool();
    }
}
