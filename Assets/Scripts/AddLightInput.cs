using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddLightInput : MonoBehaviour {

    [SerializeField]
    AddLightTool addLightTool;

    private void Update() {
        addLightTool.DoAddLight();
    }

}