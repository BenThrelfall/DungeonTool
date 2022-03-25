using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUpdater : MonoBehaviour, IMapUpdater {

    [SerializeField]
    SyncedRuntimeSprite mapSpriteSync;

    public void UpdateMap(string hash) {
        mapSpriteSync.SetHash(hash);
    }
}
