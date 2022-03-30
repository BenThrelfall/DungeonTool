using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleRulerInput : MonoBehaviour {

    [SerializeField]
    CircleRulerTool tool;
    void Update() {
        tool.PlaceRulerTool();
    }

}

