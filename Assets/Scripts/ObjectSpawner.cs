using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IObjectSpawner;

/// <summary>
/// Implementation of <c>IObjectSpawner</c>. Uses Commands to send requests between the client and the server
/// </summary>
public class ObjectSpawner : NetworkBehaviour, IObjectSpawner, ISaveablesManager {

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

        GameObject spawnedObject;

        if (spawnType == SpawnType.playerToken) {
            spawnedObject = Instantiate(playerTokenPrefab, position, rotation);
        }
        else if (spawnType == SpawnType.token) {
            spawnedObject = Instantiate(tokenPrefab, position, rotation);
        }
        else if (spawnType == SpawnType.terrainBox) {
            spawnedObject = Instantiate(terrainBoxPrefab, position, rotation);
        }
        else if(spawnType == SpawnType.map) {
            spawnedObject = Instantiate(mapPrefab, position, rotation);
        }
        else {
            throw new NotImplementedException();
        }

        spawnedObjects.Add(spawnedObject);

        spawnedObject.transform.localScale = scale;
        NetworkServer.Spawn(spawnedObject);
        if (spawnType == SpawnType.terrainBox) return;
        var spriteSync = spawnedObject.GetComponent<SyncedRuntimeSprite>();
        spriteSync.targetHash = hash;
    }

    public void SpawnObject(SpawnType type, string hash) {
        SpawnObject(type, hash, Vector3.zero, Quaternion.identity, Vector3.one);
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

    public IEnumerable<SaveData> GetActiveSaveData() {
        List<SaveData> output = new List<SaveData>();

        foreach (var gameObj in spawnedObjects) {
            var save = gameObj.GetComponent<ISaveable>();
            if (save == null) continue;
            output.Add(save.SaveData);
        }

        return output;

    }

    public void LoadFromSaveData(IEnumerable<SaveData> saveables) {

        DespawnAllSpawnedObjects();

        if (saveables == null) return;

        foreach (var saveData in saveables) {
            SpawnObject(saveData.ObjectType, saveData.SpriteHash, saveData.Position, Quaternion.identity, saveData.Scale);
        }

    }

    private void DespawnAllSpawnedObjects() {
        foreach (var gameObj in spawnedObjects) {
            NetworkServer.Destroy(gameObj);
        }

        spawnedObjects.Clear();
    }
}
