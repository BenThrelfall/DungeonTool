using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Behavour updates the sprite on its GameObject based on the provided sprite hash.
/// Will also sync the hash across the network so that all instances on the network will
/// recieve the same sprite
/// </summary>
public class SyncedRuntimeSprite : NetworkBehaviour, IRequiresDependancy {

    ISpriteCollection spriteCollection;

    [SerializeField]
    SpriteRenderer spriteRenderer;
    Image image;

    [SyncVar(hook = nameof(SpriteHashSet))]
    public string targetHash;

    [SerializeField]
    bool dontAutoDependancies;

    public void SetUpDependancies(ServiceCollection serviceCollection) {
        spriteCollection = serviceCollection.GetService<ISpriteCollection>();

        if (string.IsNullOrEmpty(targetHash) == false) StartCoroutine(WaitAndUpdateSprite());
    }

    private void Start() {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        image = GetComponent<Image>();
        if (dontAutoDependancies == false) SetUpDependancies(DependancyInjector.instance.Services);
    }

    /// <summary>
    /// Set the <c>targetHash</c> for this instance which determines what 
    /// sprite will be set for the instances GameObject.
    /// </summary>
    /// <param name="hash">Hash of the target sprite</param>
    [Command(requiresAuthority = false)]
    public void SetHash(string hash) {
        targetHash = hash;
    }

    void SpriteHashSet(string oldHash, string newHash) {
        StartCoroutine(WaitAndUpdateSprite());
    }

    IEnumerator WaitAndUpdateSprite() {

        while (spriteCollection == null) {
            yield return null;
        }

        Sprite sprite = spriteCollection.GetSprite(targetHash);

        while (sprite == null) {
            yield return new WaitForSecondsRealtime(0.2f);
            sprite = spriteCollection.GetSprite(targetHash);
        }

        if (spriteRenderer != null) spriteRenderer.sprite = sprite;
        if (image != null) image.sprite = sprite;

    }

}
