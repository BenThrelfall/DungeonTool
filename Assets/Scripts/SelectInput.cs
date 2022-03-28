using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectInput : MonoBehaviour {

    [SerializeField]
    SelectTool selectTool;

    private void Update() {
        selectTool.DoSelection();
    }

}
