using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteCollection : NetworkBehaviour, ISpriteCollection, IRequiresDependancy {

    IImageDataCollection imageCollection;

    Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

    public readonly struct NetworkSprite : IEquatable<NetworkSprite> {
        public readonly string hash;
        public readonly float pixelsPerUnit;

        public NetworkSprite(string hash, float pixelsPerUnit) {
            this.hash = hash;
            this.pixelsPerUnit = pixelsPerUnit;
        }

        public bool Equals(NetworkSprite other) {
            return hash == other.hash && pixelsPerUnit == other.pixelsPerUnit;
        }
    }

    public override void OnStartClient() {
        base.OnStartClient();
        CmdSyncToNewClient();
    }

    private void Start() {
        SetUpDependancies(DependancyInjector.instance.Services);
    }

    [ClientRpc]
    private void ClientOnNetworkSprite(NetworkSprite data) {
        if (sprites.ContainsKey(data.hash)) return;
        StartCoroutine(HandleSpriteRecival(data, false));
    }

    [Command(requiresAuthority = false)]
    private void ServerOnNetworkSprite(NetworkSprite data) {
        StartCoroutine(HandleSpriteRecival(data, true));
    }

    [Command(requiresAuthority = false)]
    void CmdSyncToNewClient() {

        foreach (var spritePair in sprites) {
            byte[] data = imageCollection.GetImage(spritePair.Key);
            ClientOnNetworkSprite(new NetworkSprite(spritePair.Key, spritePair.Value.pixelsPerUnit));
        }

    }

    IEnumerator HandleSpriteRecival(NetworkSprite data, bool isServer) {

        byte[] imageData = imageCollection.GetImage(data.hash);

        while (imageData == null) {
            yield return new WaitForSecondsRealtime(1f);
            imageData = imageCollection.GetImage(data.hash);
        }

        AddSpriteLocally(imageData, data.hash, data.pixelsPerUnit);

        if (isServer) {
            ClientOnNetworkSprite(data);
        }

    }

    public Sprite GetSprite(string hash) {
        if (sprites.ContainsKey(hash) == false) return null;
        return sprites[hash];
    }

    public void AddSprite(byte[] imageData, string hash, int pixelsPerUnit) {

        if (sprites.ContainsKey(hash)) return;

        AddSpriteLocally(imageData, hash, pixelsPerUnit);

        ServerOnNetworkSprite(new NetworkSprite(hash, pixelsPerUnit));
        imageCollection.AddImage(imageData, hash);

    }

    private void AddSpriteLocally(byte[] imageData, string hash, float pixelsPerUnit) {
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
        imageCollection = serviceCollection.GetService<IImageDataCollection>();
    }
}
