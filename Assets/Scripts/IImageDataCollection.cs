using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Image data collections hold the raw image data that is used to create sprites.
/// They should sync this data across the network.
/// </summary>
public interface IImageDataCollection {

    /// <summary>
    /// Add an image to the collection.
    /// It should be synced across the network
    /// </summary>
    /// <param name="imageData">Raw image data</param>
    /// <param name="hash">Hash of the raw data</param>
    void AddImage(byte[] imageData, string hash);
    
    /// <summary>
    /// Gets an image from the image collection.
    /// Requires polling if the image has not yet finished syncing
    /// </summary>
    /// <param name="hash">Hash of the image to retrieve</param>
    /// <returns>Image data or <c>null</c> if not found</returns>
    /// <remarks>
    /// Code calling this method loop and check until the returned value
    /// stops being null if they suspect that the image could still be syncing 
    /// across the network.
    /// </remarks>
    byte[] GetImage(string hash);


}