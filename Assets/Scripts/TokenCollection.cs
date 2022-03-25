using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenCollection : MonoBehaviour, IRequiresDependancy {

    [SerializeField]
    GameObject tokenPrefab;

    [SerializeField]
    GameObject tokenUI;

    ISpriteCollection spriteCollection;
    List<string> tokens = new List<string>();

    public void AddToken(byte[] data) {

        string hash = data.GetHashSHA1();

        tokens.Add(hash);
        spriteCollection.AddSprite(data, hash, 100);

        var token = Instantiate(tokenPrefab, tokenUI.transform);
        var runtimeSprite = token.GetComponent<LocalRuntimeSprite>();
        runtimeSprite.SetSpriteHash(hash);
        var button = token.GetComponent<UIAddTokenToBoardButton>();
        button.tokenHash = hash;

    }


    public void SetUpDependancies(ServiceCollection serviceCollection) {
        spriteCollection = serviceCollection.GetService<ISpriteCollection>();
    }
}
