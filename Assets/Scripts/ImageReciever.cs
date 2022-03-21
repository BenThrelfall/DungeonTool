using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ImageUploader;

public class ImageReciever : MonoBehaviour, IRequiresDependancy {

    Dictionary<string, byte[]> images = new Dictionary<string, byte[]>();
    IImageFileIO fileIO;

    void Start() {
        NetworkServer.RegisterHandler<ImagePart>(OnImage);
        NetworkServer.RegisterHandler<ImageUploadRequest>(OnImageRequest);
    }

    private void OnImageRequest(NetworkConnectionToClient client, ImageUploadRequest request) {
        images.Add(request.hash, new byte[request.totalSize]);
    }

    private void OnImage(NetworkConnectionToClient client, ImagePart image) {

        IList<byte> list = image.data;

        for (int i = 0; i < list.Count; i++) {
            images[image.hash][image.offset + i] = list[i];

            if (image.offset + i == images[image.hash].Length - 1) {
                UploadComplete(image.hash);
                return;
            }
        }

    }

    private void UploadComplete(string hash) {
        fileIO.SaveGameImage(hash, images[hash]);
    }

    public void SetUpDependancies(ServiceCollection serviceCollection) {
        fileIO = serviceCollection.GetService<IImageFileIO>();
    }
}
