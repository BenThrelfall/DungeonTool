using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implementation of <c>IMapUpdater</c>
/// </summary>
public class MapUpdater : NetworkBehaviour, IMapUpdater {

    [SerializeField]
    SyncedRuntimeSprite mapSpriteSync;

    [SerializeField]
    GameObject mapObject;

    public void UpdateMapSize(Vector2 size) {
        CmdUpdateMapSize(size);
    }

    public void UpdateMap(string hash) {
        mapSpriteSync.SetHash(hash);
    }

    [Command(requiresAuthority = false)]
    void CmdUpdateMapSize(Vector2 size) {
        mapObject.transform.localScale = new Vector3(size.x, size.y, 1);
        RpcUpdateMapSize(size);
    }

    [ClientRpc]
    void RpcUpdateMapSize(Vector2 size) {
        mapObject.transform.localScale = new Vector3(size.x, size.y, 1);
    }

}
