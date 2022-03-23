using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteCollection : MonoBehaviour, ISpriteCollection, IRequiresDependancy {

    IImageSender sender;
    IImageReciever reciever;

    Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

    public struct NetworkSprite : NetworkMessage {
        public string hash;
        public int pixelsPerUnit;
    }

    private void Start() {
        NetworkServer.RegisterHandler<NetworkSprite>(ServerOnNetworkSprite);

        NetworkClient.RegisterHandler<NetworkSprite>(ClientOnNetworkSprite);
    }

    private void ClientOnNetworkSprite(NetworkSprite data) {
        if (sprites.ContainsKey(data.hash)) return;
        StartCoroutine(HandleSpriteRecival(data, false));
    }

    private void ServerOnNetworkSprite(NetworkConnectionToClient client, NetworkSprite data) {
        StartCoroutine(HandleSpriteRecival(data, true));
    }

    IEnumerator HandleSpriteRecival(NetworkSprite data, bool isServer) {

        byte[] imageData = reciever.GetImage(data.hash);

        while (imageData == null) {
            yield return new WaitForSecondsRealtime(1f);
            imageData = reciever.GetImage(data.hash);
        }

        AddSpriteLocally(imageData, data.hash, data.pixelsPerUnit);

        if (isServer) {
            NetworkServer.SendToAll(data);
            sender.SendToAllClients(imageData, data.hash);
        }

    }

    public Sprite GetSprite(string hash) {
        if (sprites.ContainsKey(hash) == false) return null;
        return sprites[hash];
    }

    public void AddSprite(byte[] imageData, string hash, int pixelsPerUnit) {

        if (sprites.ContainsKey(hash)) return;

        AddSpriteLocally(imageData, hash, pixelsPerUnit);

        NetworkClient.Send(new NetworkSprite { hash = hash, pixelsPerUnit = pixelsPerUnit });
        sender.SendToServer(imageData, hash);

    }

    private void AddSpriteLocally(byte[] imageData, string hash, int pixelsPerUnit) {
        if (sprites.ContainsKey(hash)) return;

        Texture2D texture2D = CreateTexture(imageData);
        Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
        sprites.Add(hash, sprite);
    }

    Texture2D CreateTexture(byte[] data) {

        Texture2D Tex2D;
        Tex2D = new Texture2D(2, 2);
        if (Tex2D.LoadImage(data))
            return Tex2D;

        return null;

    }

    public void SetUpDependancies(ServiceCollection serviceCollection) {
        sender = serviceCollection.GetService<IImageSender>();
        reciever = serviceCollection.GetService<IImageReciever>();
    }
}
