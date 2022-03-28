using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpriteCollection {

    Sprite GetSprite(string hash);
    void AddSprite(byte[] imageData, string hash);

}
