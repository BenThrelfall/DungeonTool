using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IObjectSpawner;

/// <summary>
/// Implementation of <c>IObjectSpawner</c>. Uses <c>NetworkMessage</c> to send requests between the client and the server
/// </summary>
public class ObjectSpawner : MonoBehaviour, IObjectSpawner {

    [SerializeField]
    GameObject tokenPrefab;

    public struct ObjectSpawnRequest : NetworkMessage {
        public SpawnType spawnType;
        public string hash;
    }

    void Start() {
        NetworkServer.RegisterHandler<ObjectSpawnRequest>(ServerHandleSpawnRequest);
    }

    private void ServerHandleSpawnRequest(NetworkConnectionToClient client, ObjectSpawnRequest request) {

        if (request.spawnType != SpawnType.token) throw new NotImplementedException();

        var token = Instantiate(tokenPrefab, Vector3.zero, Quaternion.identity);
        NetworkServer.Spawn(token);
        var spriteSync = token.GetComponent<SyncedRuntimeSprite>();
        spriteSync.targetHash = request.hash;
    }

    public void SpawnObject(SpawnType type, string hash) {

        ObjectSpawnRequest request = new ObjectSpawnRequest() {
            hash = hash,
            spawnType = type,
        };

        NetworkClient.Send(request);

    }
}
