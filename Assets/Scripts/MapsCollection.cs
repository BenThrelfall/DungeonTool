using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        spriteCollection.AddSprite(data, hash, 100);

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
