using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ObjectSaveData {

    public IObjectSpawner.SpawnType spawnType;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
    public List<CompSaveData> componentData;

}