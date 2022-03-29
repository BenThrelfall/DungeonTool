using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implementation of <c>IImageDataCollection</c>
/// Holds the raw data of the images used for sprites
/// Syncs using RPCs and Commands
/// </summary>
public class ImageDataCollection : NetworkBehaviour, IImageDataCollection {

    Dictionary<string, RecievedImage> imageDatas = new Dictionary<string, RecievedImage>();

    /// <summary>
    /// Number of bytes to send to the server or client in a single 
    /// function call
    /// </summary>
    /// <remarks>
    /// Required because there is a packet size limit on 
    /// the Networkmanager
    /// </remarks>
    const int BUFFERSIZE = 14384;

    public override void OnStartClient() {
        base.OnStartClient();
        CmdSyncToNewClient();
    }

    /// <summary>
    /// Called from the client on the server when the client joins.
    /// Syncs the images currently on the server with the new client
    /// </summary>
    /// <param name="conn">Connection to the requesting client (auto-fills)</param>
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

    /// <summary>
    /// Coroutine that runs through an array of image data and sends it to the server in chunks
    /// </summary>
    /// <param name="imageData">Image data to be transfered to server</param>
    /// <param name="hash">Hash of the image data</param>
    IEnumerator TransferImageDataToServer(byte[] imageData, string hash) {

        CmdImageTransferRequest(imageData.Length, hash);

        for (int i = 0; i < imageData.Length; i+=BUFFERSIZE) {
            yield return null;
            ArraySegment<byte> segment = new ArraySegment<byte>(imageData, i, imageData.Length - i > BUFFERSIZE ? BUFFERSIZE : imageData.Length - i);
            CmdRecieveImageSection(hash, i, segment);
        }

    }

    /// <summary>
    /// Coroutine that runs through an array of image data and sends it to all clients in chunks
    /// </summary>
    /// <param name="imageData">Image data to be sent to the clients</param>
    /// <param name="hash">Hash of the image data</param>
    IEnumerator TransferImageDataToClients(byte[] imageData, string hash) {
        RpcImageTransferRequest(imageData.Length, hash);

        for (int i = 0; i < imageData.Length; i+=BUFFERSIZE) {
            yield return null;
            ArraySegment<byte> segment = new ArraySegment<byte>(imageData, i, imageData.Length - i > BUFFERSIZE ? BUFFERSIZE : imageData.Length - i);
            RpcRecieveImageSection(hash, i, segment);
        }
    }

    /// <summary>
    /// Coroutine that runs through an array of image data and sends it to a specific client in chunks
    /// </summary>
    /// <param name="target">Client to send data to</param>
    /// <param name="imageData">Data to be sent to the client</param>
    /// <param name="hash">Hash of the image data</param>
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

    /// <summary>
    /// Simple class for having a <c>Compete</c> flag together with image data
    /// </summary>
    private class RecievedImage {
        public bool Complete { get; set; } = false;
        public byte[] Data { get; private set; }

        public RecievedImage(byte[] data) {
            Data = data;
        }
    }

}
