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
        token
    }

    /// <summary>
    /// Spawn an object of the given <c>SpawnType</c> on the server. 
    /// </summary>
    /// <param name="type">Type of object to spawn</param>
    /// <param name="hash">Hash that will be provided to the spawned objects SpriteSync component</param>
    public void SpawnObject(SpawnType type, string hash);
}
