using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns objects on the server
/// </summary>
public interface IObjectSpawner {

    /// <summary>
    /// Type of object to be spawned
    /// </summary>
    public enum SpawnType {
        token,
        playerToken,
        terrainBox,
        map,
        fog
    }

    /// <summary>
    /// Spawn an object of the given <c>SpawnType</c> on the server. 
    /// </summary>
    /// <param name="type">Type of object to spawn</param>
    /// <param name="hash">Hash that will be provided to the spawned objects SpriteSync component</param>
    public void SpawnObject(SpawnType type, string hash);
    public void SpawnObject(SpawnType type, string hask, Vector3 position);
    public void SpawnObject(SpawnType type, string hash, Vector3 position, Quaternion rotation, Vector3 scale);
    public void SpawnFromObjectData(IEnumerable<ObjectSaveData> data);
    public void DespawnAllSpawnedObjects();
    public void DespawnObject(GameObject gameObject);
}
