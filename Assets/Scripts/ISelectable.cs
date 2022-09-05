using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An object selectable by the selection system
/// </summary>
public interface ISelectable { 

    Bounds ObjectBounds { get; }
    void Delete();

    /// <summary>
    /// Updates the position of the object locally: without syncing across the network.
    /// </summary>
    /// <param name="delta">Change in position from the last position recieved from the server</param>
    void DragPosition(Vector2 delta);
    /// <summary>
    /// Updates the objects position on the server to it's current local position.
    /// </summary>
    void Move();

    /// <summary>
    /// Updates the size of an object locally: without syncing across the network.
    /// </summary>
    /// <param name="newSize">New size for the object</param>
    void DragSize(Vector2 newSize);
    /// <summary>
    /// Changes the objects size and pushes changes across the network
    /// </summary>
    /// <param name="newSize">New size for the object</param>
    void ResizeWithSnapping(Vector2 newSize);

    void ResizeNoSnapping(Vector2 newSize);

    /// <summary>
    /// For use on the server side
    /// </summary>
    /// <param name="newSize"></param>
    void ServerResize(Vector2 newSize);

    void IncreaseLight();

    void DecreaseLight();

}
