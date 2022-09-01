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

    [SerializeField]
    GameObject playerTokenPrefab;

    [SerializeField]
    GameObject terrainBoxPrefab;

    [SerializeField]
    GameObject mapPrefab;

    List<GameObject> spawnedObjects = new List<GameObject>();

    [Command(requiresAuthority = false)]
    void CmdServerHandleSpawnRequest(SpawnType spawnType, string hash, Vector3 position, Quaternion rotation, Vector3 scale) {

        ObjectSaveData data = new ObjectSaveData() {
            spawnType = spawnType,
            position = position,
            rotation = rotation.eulerAngles,
            scale = scale,
            componentData = new List<CompSaveData>() { new CompSaveData(CompType.SyncedRuntimeSprite) { Json = $"{hash}"} }
        };

        ServerHandleSpawnRequest(data);
    }

    [Server]
    private GameObject ServerHandleSpawnRequest(ObjectSaveData objData) {
        GameObject spawnedObject;

        SpawnType spawnType = objData.spawnType;

        if (spawnType == SpawnType.playerToken) {
            spawnedObject = Instantiate(playerTokenPrefab);
        }
        else if (spawnType == SpawnType.token) {
            spawnedObject = Instantiate(tokenPrefab);
        }
        else if (spawnType == SpawnType.terrainBox) {
            spawnedObject = Instantiate(terrainBoxPrefab);
        }
        else if (spawnType == SpawnType.map) {
            spawnedObject = Instantiate(mapPrefab);
        }
        else {
            throw new NotImplementedException();
        }

        spawnedObjects.Add(spawnedObject);
        NetworkServer.Spawn(spawnedObject);

        spawnedObject.GetComponent<SaveObject>().Load(objData);

        return spawnedObject;

    }

    private IEnumerator DelayResize(ISelectable selectable, Vector2 scale) {
        yield return null;
        yield return null;
        selectable.ServerResize(scale);
    }

    public void SpawnObject(SpawnType type, string hash) {
        SpawnObject(type, hash, Vector3.zero, Quaternion.identity, Vector3.one);
    }
    public void SpawnObject(SpawnType type, string hash, Vector3 position) {
        SpawnObject(type, hash, position, Quaternion.identity, Vector3.one);
    }

    public void SpawnObject(SpawnType type, string hash, Vector3 position, Quaternion rotation, Vector3 scale) {
        CmdServerHandleSpawnRequest(type, hash,position, rotation, scale);
    }

    public void DespawnObject(GameObject gameObject) {
        CmdDespawnObject(gameObject);
    }

    [Command(requiresAuthority = false)]
    void CmdDespawnObject(GameObject gameObject) {
        spawnedObjects.Remove(gameObject);
        NetworkServer.Destroy(gameObject);
    }


    [Server]
    public void SpawnFromObjectData(IEnumerable<ObjectSaveData> data) {

        if (data == null) return;

        foreach (var obj in data) {
            ServerHandleSpawnRequest(obj);
        }

    }

    public void DespawnAllSpawnedObjects() {
        foreach (var gameObj in spawnedObjects) {
            NetworkServer.Destroy(gameObj);
        }

        spawnedObjects.Clear();
    }


}
