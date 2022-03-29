using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behavour that goes on each UI element that can change the sprite of the
/// board map. One sprite is assigned per instance of this behavour.
/// </summary>
public class UIChangeMapButton : MonoBehaviour, IRequiresDependancy {

    /// <summary>
    /// Hash of the sprite that will be used for updating the board
    /// </summary>
    public string boardHash;

    IMapUpdater mapUpader;

    [SerializeField]
    bool dontAutoDependancies;

    private void Start() {
        if (dontAutoDependancies == false) SetUpDependancies(DependancyInjector.instance.Services);
    }

    /// <summary>
    /// Change the board sprite to be the sprite assigned to this behavour instance
    /// </summary>
    public void OnClick() {
        mapUpader.UpdateMap(boardHash);
    }

    public void SetUpDependancies(ServiceCollection serviceCollection) {
        mapUpader = serviceCollection.GetService<IMapUpdater>();
    }
}
