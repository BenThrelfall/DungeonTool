using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageDataCollection : NetworkBehaviour, IImageDataCollection {

    Dictionary<string, RecievedImage> imageDatas = new Dictionary<string, RecievedImage>();
    const int BUFFERSIZE = 14384;

    public override void OnStartClient() {
        base.OnStartClient();
        CmdSyncToNewClient();
    }

    [Command(requiresAuthority = false)]
    private void CmdSyncToNewClient(NetworkConnectionToClient conn = null) {
        foreach (var pair in imageDatas) {
            StartCoroutine(TransferImageDataToClient(conn, pair.Value.Data, pair.Key));
        }
    }

    public void AddImage(byte[] imageData, string hash) {
        if (isServer) {
            AddImageLocally(imageData, hash);
            StartCoroutine(TransferImageDataToClients(imageDatas[hash].Data, hash));
        }
        else {
            AddImageLocally(imageData, hash);
            StartCoroutine(TransferImageDataToServer(imageData, hash));
        }
    }

    public byte[] GetImage(string hash) {
        if (imageDatas.ContainsKey(hash) == false) return null;

        if (imageDatas[hash].Complete == false) return null;

        return imageDatas[hash].Data;
    }

    IEnumerator TransferImageDataToServer(byte[] imageData, string hash) {

        CmdImageTransferRequest(imageData.Length, hash);

        for (int i = 0; i < imageData.Length; i+=BUFFERSIZE) {
            yield return null;
            ArraySegment<byte> segment = new ArraySegment<byte>(imageData, i, imageData.Length - i > BUFFERSIZE ? BUFFERSIZE : imageData.Length - i);
            CmdRecieveImageSection(hash, i, segment);
        }

    }

    IEnumerator TransferImageDataToClients(byte[] imageData, string hash) {
        RpcImageTransferRequest(imageData.Length, hash);

        for (int i = 0; i < imageData.Length; i+=BUFFERSIZE) {
            yield return null;
            ArraySegment<byte> segment = new ArraySegment<byte>(imageData, i, imageData.Length - i > BUFFERSIZE ? BUFFERSIZE : imageData.Length - i);
            RpcRecieveImageSection(hash, i, segment);
        }
    }

    IEnumerator TransferImageDataToClient(NetworkConnection target, byte[] imageData, string hash) {
        TargetImageTransferRequest(target, imageData.Length, hash);

        for (int i = 0; i < imageData.Length; i += BUFFERSIZE) {
            yield return null;
            ArraySegment<byte> segment = new ArraySegment<byte>(imageData, i, imageData.Length - i > BUFFERSIZE ? BUFFERSIZE : imageData.Length - i);
            TargetRecieveImageSection(target, hash, i, segment);
        }
    }

    [Command(requiresAuthority = false)]
    void CmdImageTransferRequest(int imageSize, string hash) {
        AllocateImage(imageSize, hash);
    }

    [Command(requiresAuthority = false)]
    void CmdRecieveImageSection(string hash, int offset, ArraySegment<byte> data) {
        InsertDataIntoImages(hash, offset, data);

    }

    [ClientRpc]
    void RpcImageTransferRequest(int imageSize, string hash) {
        AllocateImage(imageSize, hash);
    }

    [ClientRpc]
    void RpcRecieveImageSection(string hash, int offset, ArraySegment<byte> data) {
        InsertDataIntoImages(hash, offset, data);
    }

    [TargetRpc]
    void TargetImageTransferRequest(NetworkConnection target, int imageSize, string hash) {
        AllocateImage(imageSize, hash);
    }

    [TargetRpc]
    void TargetRecieveImageSection(NetworkConnection target, string hash, int offset, ArraySegment<byte> data) {
        InsertDataIntoImages(hash, offset, data);
    }

    private void InsertDataIntoImages(string hash, int offset, ArraySegment<byte> data) {

        if (imageDatas[hash].Complete) return;

        IList<byte> list = data;

        for (int i = 0; i < list.Count; i++) {
            imageDatas[hash].Data[offset + i] = list[i];

            if (offset + i == imageDatas[hash].Data.Length - 1) {
                UploadComplete(hash);
                return;
            }
        }
    }

    private void UploadComplete(string hash) {
        imageDatas[hash].Complete = true;

        if (isServer) {
            StartCoroutine(TransferImageDataToClients(imageDatas[hash].Data, hash));
        }
    }

    private void AllocateImage(int imageSize, string hash) {
        if (imageDatas.ContainsKey(hash)) return;
        imageDatas.Add(hash, new RecievedImage(new byte[imageSize]));
    }

    void AddImageLocally(byte[] imageData, string hash) {
        if (imageDatas.ContainsKey(hash)) return;

        imageDatas.Add(hash, new RecievedImage(imageData));
        imageDatas[hash].Complete = true;

    }

    private class RecievedImage {
        public bool Complete { get; set; } = false;
        public byte[] Data { get; private set; }

        public RecievedImage(byte[] data) {
            Data = data;
        }
    }

}
