using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Collection of tokens that can be added to the board by the local client.
/// When a token is added its data is added to the sprite collection to be synced
/// </summary>
/// <remarks>
/// The actual collection is unused as determining what tokens can be added is currently
/// done entirely through what UI elements have been created. At the moment it just acts
/// as the middle man for adding tokens to the UI and the sprite collection.
/// </remarks>
public class TokenCollection : MonoBehaviour, IRequiresDependancy {

    [SerializeField]
    GameObject tokenPrefab;

    [SerializeField]
    GameObject tokenUI;

    ISpriteCollection spriteCollection;
    List<string> tokens = new List<string>();

    /// <summary>
    /// Add a token to the collection.
    /// Will add a sprite to the sprite collection using <c>data</c>
    /// Will add a button to the UI for the token
    /// </summary>
    /// <param name="data">Raw image data for the token</param>
    public void AddToken(byte[] data) {

        string hash = data.GetHashSHA1();

        tokens.Add(hash);
        spriteCollection.AddSprite(data, hash);

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
