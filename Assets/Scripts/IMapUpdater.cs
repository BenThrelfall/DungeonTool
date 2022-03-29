using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMapUpdater {
    void UpdateMapSize(Vector2 size);
    void UpdateMap(string hash);
}