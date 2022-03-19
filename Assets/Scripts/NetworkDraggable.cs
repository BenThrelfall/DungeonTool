using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkDraggable : NetworkBehaviour {

    Camera mainCamera;

    private void Start() {
        mainCamera = Camera.main;
    }

    private void OnMouseDrag() {
        var worldPoint = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        CmdUpdateTokenPos(worldPoint);
    }

    [Command(requiresAuthority = false)]
    private void CmdUpdateTokenPos(Vector3 position) {
        transform.position = new Vector3(position.x, position.y, 0);
    }

    private void OnMouseUp() {
        var roundedPos = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), 0);
        CmdUpdateTokenPos(roundedPos);
    }

}
