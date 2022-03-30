using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnRightClick : NetworkBehaviour {

    private void Update() {
        if (Input.GetMouseButtonDown(1)) {
            CmdDestroy();
        }
    }

    [Command(requiresAuthority = false)]
    private void CmdDestroy() {
        NetworkServer.Destroy(gameObject);
    }
}
