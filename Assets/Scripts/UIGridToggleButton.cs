using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behavour that goes on the UI element that toggles the grid on or off.
/// </summary>
public class UIGridToggleButton : MonoBehaviour {

    [SerializeField]
    GameObject grid;

    /// <summary>
    /// Toggles the state of the grid on or off
    /// </summary>
    public void OnClick() {
        grid.SetActive(!grid.activeSelf);
    }

}
