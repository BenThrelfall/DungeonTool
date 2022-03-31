using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class UserRoleManager : NetworkBehaviour {

    bool usersHaveJoined = false;

    [SerializeField]
    RenderPipelineAsset dmRenderAsset;

    [SerializeField]
    List<GameObject> dmObjects = new List<GameObject>();

    public override void OnStartClient() {
        base.OnStartClient();

        CmdUserJoined();
    }

    [Command(requiresAuthority = false)]
    void CmdUserJoined(NetworkConnectionToClient conn = null) {
        if (usersHaveJoined) {
            TargetSetAsPlayer(conn);
        }
        else {
            TargetSetAsDM(conn);
            usersHaveJoined = true;
        }
    }

    [TargetRpc]
    void TargetSetAsPlayer(NetworkConnection conn) {

    }

    [TargetRpc]
    void TargetSetAsDM(NetworkConnection conn) {

        foreach (var obj in dmObjects) {
            obj.SetActive(true);
        }

        GraphicsSettings.renderPipelineAsset = dmRenderAsset;

    }


}
