using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerInput : MonoBehaviour {

    [SerializeField]
    PointerTool pointerTool;

    void Update() {
        pointerTool.PlacePointerTool();
    }
}
