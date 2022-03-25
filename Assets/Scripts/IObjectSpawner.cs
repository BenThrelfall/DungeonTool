using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectSpawner {

    public enum SpawnType {
        token
    }

    public void SpawnObject(SpawnType type, string hash);
}
