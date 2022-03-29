using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Runs the main method of <c>RulerTool</c>. Used by the tool system
/// to activate or deactive the tool
/// </summary>
/// <remarks>
/// In the future all input caputre for the tool may be moved into this class
/// </remarks>
public class RulerInput : MonoBehaviour {

    [SerializeField]
    RulerTool tool;
    void Update() {
        tool.PlaceRulerTool();
    }
}
