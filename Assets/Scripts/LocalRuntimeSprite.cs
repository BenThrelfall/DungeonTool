using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Sets the sprite on a gameObject based on a hash provided at runtime.
/// </summary>
public class LocalRuntimeSprite : MonoBehaviour, IRequiresDependancy {

    ISpriteCollection spriteCollection;

    SpriteRenderer spriteRenderer;
    Image image;

    [SerializeField]
    string targetHash;

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

    public void SetSpriteHash(string hash) {
        targetHash = hash;
        StartCoroutine(WaitAndUpdateSprite());
    }

    IEnumerator WaitAndUpdateSprite() {

        Sprite sprite = spriteCollection.GetSprite(targetHash);

        while (sprite == null) {
            yield return new WaitForSecondsRealtime(0.2f);
            sprite = spriteCollection.GetSprite(targetHash);
        }

        if(spriteRenderer != null) spriteRenderer.sprite = sprite;
        if(image != null) image.sprite = sprite;

    }

}
