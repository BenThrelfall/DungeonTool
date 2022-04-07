using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSaveable : MonoBehaviour, ISaveable {

    public SaveData SaveData {
        get => new SaveData() {
            SpriteHash = spriteSync == null ? "" : spriteSync.targetHash,
            ObjectType = spawnType,
            Position = transform.position,
            Scale = scaleTrans.localScale
        };
    }

    [SerializeField]
    IObjectSpawner.SpawnType spawnType;

    [SerializeField]
    Transform scaleTrans;

    SyncedRuntimeSprite spriteSync;

    void Start() {
        spriteSync = GetComponent<SyncedRuntimeSprite>();
    }

}
