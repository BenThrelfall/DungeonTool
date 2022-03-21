using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DebugOpenImage : MonoBehaviour {

    string filePath;

    [SerializeField]
    ImageUploader uploader;

    void OnGUI() {
        GUILayout.BeginArea(new Rect(500, 200, 215, 9999));

        filePath = GUILayout.TextField(filePath);

        if (GUILayout.Button("Open File")) {
            uploader.OpenAndSendImageFromFile(filePath);
        }

        GUILayout.EndArea();
    }

}
