using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static IObjectSpawner;

[Serializable]
public struct SaveData {

    public string SpriteHash;
    public SpawnType ObjectType;
    public Vector3 Position;
    public Vector3 Scale;

}