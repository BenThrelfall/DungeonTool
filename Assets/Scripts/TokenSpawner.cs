using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenSpawner : NetworkBehaviour {

    [SerializeField]
    GameObject token;

    void OnGUI() {
        GUILayout.BeginArea(new Rect(500, 40, 215, 9999));

        if (GUILayout.Button("Spawn")) {
            CmdSpawnToken();
        }

        GUILayout.EndArea();
    }

    [Command(requiresAuthority=false)]
    private void CmdSpawnToken() {
        var instance = Instantiate(token);
        NetworkServer.Spawn(instance);
    }
}
