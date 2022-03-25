using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SyncedRuntimeSprite : NetworkBehaviour, IRequiresDependancy {

    ISpriteCollection spriteCollection;

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
        spriteRenderer = GetComponent<SpriteRenderer>();
        image = GetComponent<Image>();
        if (dontAutoDependancies == false) SetUpDependancies(DependancyInjector.instance.Services);
    }

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
