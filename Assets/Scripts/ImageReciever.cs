using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ImageSender;

public class ImageReciever : MonoBehaviour, IRequiresDependancy, IImageReciever {

    Dictionary<string, RecievedImage> images = new Dictionary<string, RecievedImage>();

    public byte[] GetImage(string hash) {
        if (images.ContainsKey(hash) == false) return null;

        if (images[hash].Complete == false) return null;

        return images[hash].Data;
    }

    void Start() {
        NetworkServer.RegisterHandler<ImagePart>(OnImageServer);
        NetworkServer.RegisterHandler<ImageUploadRequest>(OnImageRequestServer);

        NetworkClient.RegisterHandler<ImagePart>(OnImage);
        NetworkClient.RegisterHandler<ImageUploadRequest>(OnImageRequest);
    }

    private void OnImageRequest(ImageUploadRequest request) {
        AllocateImageInMemory(request);
    }

    private void OnImage(ImagePart image) {
        ProcessImage(image);
    }

    private void OnImageRequestServer(NetworkConnectionToClient client, ImageUploadRequest request) {
        AllocateImageInMemory(request);
    }

    private void AllocateImageInMemory(ImageUploadRequest request) {
        if (images.ContainsKey(request.hash)) return;
        images.Add(request.hash, new RecievedImage(new byte[request.totalSize]));
    }

    private void OnImageServer(NetworkConnectionToClient client, ImagePart image) {
        ProcessImage(image);
    }

    private void ProcessImage(ImagePart image) {

        if (images[image.hash].Complete) return;

        IList<byte> list = image.data;

        for (int i = 0; i < list.Count; i++) {
            images[image.hash].Data[image.offset + i] = list[i];

            if (image.offset + i == images[image.hash].Data.Length - 1) {
                UploadComplete(image.hash);
                return;
            }
        }
    }

    private void UploadComplete(string hash) {
        images[hash].Complete = true;
    }

    public void SetUpDependancies(ServiceCollection serviceCollection) {
    }

    private class RecievedImage {
        public bool Complete { get; set; } = false;
        public byte[] Data { get; private set; }

        public RecievedImage(byte[] data) {
            Data = data;
        }
    }

}
