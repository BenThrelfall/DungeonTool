using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Behavour that goes on the UI element that prompts
/// a user to add a map from the file system.
/// </summary>
public class UIAddMapButton : MonoBehaviour, IRequiresDependancy {

    [SerializeField]
    MapsCollection mapCollection;

    IFileIOService fileIOService;

    private void Start() {
        SetUpDependancies(DependancyInjector.instance.Services);
    }

    /// <summary>
    /// Prompts a user to choose a file from the file system then sends
    /// the bytes from that file to the map collection to be added as a new map.
    /// </summary>
    public void OnClick() {

        fileIOService.ReadAllBytes((x) => mapCollection.AddMap(x));

    }


    public void SetUpDependancies(ServiceCollection serviceCollection) {
        fileIOService = serviceCollection.GetService<IFileIOService>();
    }

}
