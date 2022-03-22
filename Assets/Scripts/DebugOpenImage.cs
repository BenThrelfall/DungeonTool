using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class DebugOpenImage : MonoBehaviour {

    string filePath;

    [SerializeField]
    SpriteCollection spriteCollection;

    void OnGUI() {
        GUILayout.BeginArea(new Rect(500, 200, 215, 9999));

        filePath = GUILayout.TextField(filePath);

        if (GUILayout.Button("Open File")) {
            var data = File.ReadAllBytes(filePath);
            spriteCollection.AddSprite(data, data.GetHashSHA1(), 100);
        }

        GUILayout.EndArea();
    }

}
