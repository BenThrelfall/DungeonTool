using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMapSizeUpdater : MonoBehaviour, IRequiresDependancy {

    IMapUpdater updater;

    Vector2 size;

    public void SetUpDependancies(ServiceCollection serviceCollection) {
        updater = serviceCollection.GetService<IMapUpdater>();
    }

    public void OnWidthFinished(string text) {
        if (int.TryParse(text, out int i)) {
            size = new Vector2(i, size.y);
        }

        UpdateMap();
    }

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
