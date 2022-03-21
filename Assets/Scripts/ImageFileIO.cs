using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ImageFileIO : IImageFileIO {

    public byte[] ReadAllImageBytes(string filePath) {
        return File.ReadAllBytes(filePath);
    }

    public void SaveGameImage(string hash, byte[] data) {
        File.WriteAllBytes($"{Application.persistentDataPath}\\{hash}.png", data);
    }
}
