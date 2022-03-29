using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Collection of user entered sprites that are available for use in the game.
/// Syncs across the network.
/// </summary>
public interface ISpriteCollection {

    /// <summary>
    /// Fetches a sprite from the collection based on a hash.
    /// Requires polling if the sprite has not synced yet.
    /// </summary>
    /// <param name="hash">Hash for the data of the sprite to be returned</param>
    /// <returns>Sprite for the specified hash or <c>null</c> if not found</returns>
    /// <remarks>
    /// Methods which call this should loop and wait if null is returned as the 
    /// sprite may still be syncing.
    /// </remarks>
    Sprite GetSprite(string hash);

    /// <summary>
    /// Creates a new sprite in the collection based on the provided data.
    /// It will be synced across the network
    /// </summary>
    /// <param name="imageData">Raw data for the sprite</param>
    /// <param name="hash">Hash of the raw data</param>
    void AddSprite(byte[] imageData, string hash);

}
