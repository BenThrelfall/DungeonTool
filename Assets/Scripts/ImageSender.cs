using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ImageSender : MonoBehaviour, IRequiresDependancy, IImageSender {

    const int BUFFERSIZE = 14384;

    public struct ImagePart : NetworkMessage {
        public string hash;
        public int offset;
        public ArraySegment<byte> data;
    }

    public struct ImageUploadRequest : NetworkMessage {
        public string hash;
        public int totalSize;
    }


    public void SendToServer(byte[] imageData, string hash) {
        StartCoroutine(SendImageToServer(imageData, hash));
    }

    public void SendToAllClients(byte[] imageData, string hash) {
        StartCoroutine(SendImageToClient(imageData, hash)); 
    }

    IEnumerator SendImageToClient(byte[] data, string hash) {

        var imageRequest = new ImageUploadRequest {
            hash = hash,
            totalSize = data.Length,
        };

        NetworkServer.SendToAll(imageRequest);

        for (int i = 0; i < data.Length; i += BUFFERSIZE) {
            yield return null;
            ArraySegment<byte> segment = new ArraySegment<byte>(data, i, data.Length - i > BUFFERSIZE ? BUFFERSIZE : data.Length - i);
            var image = new ImagePart() { hash = hash, offset = i, data = segment };
            NetworkServer.SendToAll(image);
        }
    }

    IEnumerator SendImageToServer(byte[] data, string hash) {

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

    public void SetUpDependancies(ServiceCollection serviceCollection) {
    }
}
