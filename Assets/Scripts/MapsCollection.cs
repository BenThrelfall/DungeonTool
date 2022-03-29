using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Collection of maps that are available locally. When a map is added it's added to the sprite collection
/// so it's sprite can be synced across the network
/// </summary>
/// <remarks>
/// The actual collection part of the map collection is unused. At the moment it just acts as a middle man 
/// for adding maps to the UI and sprite collection
/// </remarks>
public class MapsCollection : MonoBehaviour, IRequiresDependancy {

    [SerializeField]
    GameObject mapPrefab;

    [SerializeField]
    GameObject mapUI;

    ISpriteCollection spriteCollection;
    List<string> maps = new List<string>();

    public void AddMap(byte[] data) {

        string hash = data.GetHashSHA1();

        maps.Add(hash);
        spriteCollection.AddSprite(data, hash);

        var token = Instantiate(mapPrefab, mapUI.transform);
        var runtimeSprite = token.GetComponent<LocalRuntimeSprite>();
        runtimeSprite.SetSpriteHash(hash);
        var button = token.GetComponent<UIChangeMapButton>();
        button.boardHash = hash;

    }


    public void SetUpDependancies(ServiceCollection serviceCollection) {
        spriteCollection = serviceCollection.GetService<ISpriteCollection>();
    }
}
