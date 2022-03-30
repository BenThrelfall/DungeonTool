using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class UIConnectionStatusText : MonoBehaviour {

    [SerializeField]
    NetworkManager manager;

    [SerializeField]
    TextMeshProUGUI text;

    private void Update() {
        text.text = $"Connecting to {manager.networkAddress}..";
    }

}
