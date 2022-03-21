using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ImageUploader : MonoBehaviour, IRequiresDependancy {

    const int BUFFERSIZE = 14384;

    IImageFileIO fileIO;


    public struct ImagePart : NetworkMessage {
        public string hash;
        public int offset;
        public ArraySegment<byte> data;
    }

    public struct ImageUploadRequest : NetworkMessage {
        public string hash;
        public int totalSize;
    }

    public void OpenAndSendImageFromFile(string fileName) {
        var data = fileIO.ReadAllImageBytes(fileName);
        StartCoroutine(SendImage(data));
    }

    IEnumerator SendImage(byte[] data) {
        var hash = GetHashSHA1(data);

        var imageRequest = new ImageUploadRequest {
            hash = hash,
            totalSize = data.Length,
        };

        NetworkClient.Send(imageRequest);

        for (int i = 0; i < data.Length; i += BUFFERSIZE) {
            yield return null;
            ArraySegment<byte> segment = new ArraySegment<byte>(data, i, data.Length - i > BUFFERSIZE ? BUFFERSIZE : data.Length - i);
            var image = new ImagePart() { hash = hash, offset = i, data = segment };
            NetworkClient.Send(image);
        }
    }

    static string GetHashSHA1(byte[] data) {
        using (var sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider()) {
            return string.Concat(sha1.ComputeHash(data).Select(x => x.ToString("X2")));
        }
    }

    public void SetUpDependancies(ServiceCollection serviceCollection) {
        fileIO = serviceCollection.GetService<IImageFileIO>();
    }
}
