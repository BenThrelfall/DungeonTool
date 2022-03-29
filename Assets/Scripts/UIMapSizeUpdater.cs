using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behavour for the UI element that updates the size of the board map.
/// Updates the size of the board map when the UI value is changed
/// </summary>
public class UIMapSizeUpdater : MonoBehaviour, IRequiresDependancy {

    IMapUpdater updater;

    Vector2 size;

    public void SetUpDependancies(ServiceCollection serviceCollection) {
        updater = serviceCollection.GetService<IMapUpdater>();
    }

    /// <summary>
    /// Updates the width of the board map.
    /// Should be called when the width text has
    /// finished being editted.
    /// </summary>
    /// <param name="text">Text of new width</param>
    public void OnWidthFinished(string text) {
        if (int.TryParse(text, out int i)) {
            size = new Vector2(i, size.y);
        }

        UpdateMap();
    }

    /// <summary>
    /// Updates the height of the board map.
    /// Should be called when the height text has
    /// finished being editted.
    /// </summary>
    /// <param name="text">Text of new height</param>
    public void OnHeightFinished(string text) {
        if (int.TryParse(text, out int i)) {
            size = new Vector2(size.x, i);
        }

        UpdateMap();
    }

    void UpdateMap() {
        updater.UpdateMapSize(size);
    }
}
