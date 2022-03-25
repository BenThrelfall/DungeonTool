using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGridToggleButton : MonoBehaviour {

    [SerializeField]
    GameObject grid;

    public void OnClick() {
        grid.SetActive(!grid.activeSelf);
    }

}
