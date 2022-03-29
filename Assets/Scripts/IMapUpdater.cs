using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Updates the board map
/// </summary>
public interface IMapUpdater {

    /// <summary>
    /// Change the size of the board map
    /// </summary>
    /// <param name="size">New size for the board map in units</param>
    void UpdateMapSize(Vector2 size);

    /// <summary>
    /// Change the sprite for the board map
    /// </summary>
    /// <param name="hash">Hash of the new sprite</param>
    void UpdateMap(string hash);
}