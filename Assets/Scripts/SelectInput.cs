using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Runs the main method of <c>SelectTool</c>. Used by the tool system
/// to activate or deactive the tool
/// </summary>
/// <remarks>
/// In the future all input caputre for the tool may be moved into this class
/// </remarks>
public class SelectInput : MonoBehaviour {

    [SerializeField]
    SelectTool selectTool;

    private void Update() {
        selectTool.DoSelection();
    }

}
