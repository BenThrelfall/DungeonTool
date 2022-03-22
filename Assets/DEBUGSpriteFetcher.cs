using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUGSpriteFetcher : MonoBehaviour {

    [SerializeField]
    SpriteCollection spriteCollection;

    [SerializeField]
    SpriteRenderer spriteRenderer;

    [SerializeField]
    string targetHash;

    private void Start() {
        StartCoroutine(WaitAndUpdateSprite());
    }

    IEnumerator WaitAndUpdateSprite() {

        Sprite sprite = spriteCollection.GetSprite(targetHash);

        while (sprite == null) {
            yield return new WaitForSecondsRealtime(0.2f);
            sprite = spriteCollection.GetSprite(targetHash);
        }

        spriteRenderer.sprite = sprite;

    }

}
