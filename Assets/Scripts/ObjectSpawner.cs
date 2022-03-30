using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IObjectSpawner;

/// <summary>
/// Implementation of <c>IObjectSpawner</c>. Uses Commands to send requests between the client and the server
/// </summary>
public class ObjectSpawner : NetworkBehaviour, IObjectSpawner {

    [SerializeField]
    GameObject tokenPrefab;

    [Command(requiresAuthority = false)]
    void CmdServerHandleSpawnRequest(SpawnType spawnType, string hash) {

        if (spawnType != SpawnType.token) throw new NotImplementedException();

        var token = Instantiate(tokenPrefab, Vector3.zero, Quaternion.identity);
        NetworkServer.Spawn(token);
        var spriteSync = token.GetComponent<SyncedRuntimeSprite>();
        spriteSync.targetHash = hash;
    }

    public void SpawnObject(SpawnType type, string hash) {
        CmdServerHandleSpawnRequest(type, hash);
    }
}
