using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolUIToggle : MonoBehaviour {

    [SerializeField]
    GameObject uiObject;

    private void OnEnable() {
        uiObject.SetActive(true);
    }

}
