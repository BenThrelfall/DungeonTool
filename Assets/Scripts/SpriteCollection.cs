using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

/// <summary>
/// Implementation of <c>ISpriteCollection</c>
/// Uses commands and RPCs to sync sprites across the network
/// </summary>
public class SpriteCollection : NetworkBehaviour, ISpriteCollection, IRequiresDependancy {

    IImageDataCollection imageCollection;

    Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

    string spriteFolder;

    public readonly struct NetworkSprite : IEquatable<NetworkSprite> {
        public readonly string hash;

        public NetworkSprite(string hash) {
            this.hash = hash;
        }

        public bool Equals(NetworkSprite other) {
            return hash == other.hash;
        }
    }

    public override void OnStartClient() {
        base.OnStartClient();
        CmdSyncToNewClient();
    }

    public override void OnStartServer() {
        base.OnStartServer();

        SetUpSpriteFolder();
    }

    private void Start() {
        SetUpDependancies(DependancyInjector.instance.Services);
    }

    [Server]
    private void SetUpSpriteFolder() {
        spriteFolder = Application.persistentDataPath + "/sprites/";
        if (Directory.Exists(spriteFolder) == false) Directory.CreateDirectory(spriteFolder);
    }

    [ClientRpc]
    private void RpcClientOnNetworkSprite(NetworkSprite data) {
        if (sprites.ContainsKey(data.hash)) return;
        StartCoroutine(HandleSpriteRecival(data, false));
    }

    [Command(requiresAuthority = false)]
    private void CmdServerOnNetworkSprite(NetworkSprite data) {
        StartCoroutine(HandleSpriteRecival(data, true));
    }

    [Command(requiresAuthority = false)]
    void CmdSyncToNewClient() {

        foreach (var spritePair in sprites) {
            byte[] data = imageCollection.GetImage(spritePair.Key);
            RpcClientOnNetworkSprite(new NetworkSprite(spritePair.Key));
        }

    }

    IEnumerator HandleSpriteRecival(NetworkSprite data, bool isServer) {

        byte[] imageData = imageCollection.GetImage(data.hash);

        while (imageData == null) {
            yield return new WaitForSecondsRealtime(1f);
            imageData = imageCollection.GetImage(data.hash);
        }

        AddSpriteLocally(imageData, data.hash);

        if (isServer) {
            ServerAfterHandleSpriteRecival(data, imageData);
        }

    }

    [Server]
    private void ServerAfterHandleSpriteRecival(NetworkSprite data, byte[] imageData) {
        File.WriteAllBytes($"{spriteFolder}{data.hash}.png", imageData);
        RpcClientOnNetworkSprite(data);
    }

    public Sprite GetSprite(string hash) {
        if (sprites.ContainsKey(hash) == false) return null;
        return sprites[hash];
    }

    public void AddSprite(byte[] imageData, string hash) {

        if (sprites.ContainsKey(hash)) return;

        AddSpriteLocally(imageData, hash);

        CmdServerOnNetworkSprite(new NetworkSprite(hash));
        imageCollection.AddImage(imageData, hash);

    }

    private void AddSpriteLocally(byte[] imageData, string hash) {
        if (sprites.ContainsKey(hash)) return;

        Texture2D texture2D = CreateTexture(imageData);
        var pixelsPerUnit = (texture2D.width > texture2D.height ? texture2D.width : texture2D.height);
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

    [Server]
    public void LoadSpriteFromStorage(string hash) {
        if (sprites.Keys.Contains(hash)) return;
        if (File.Exists($"{spriteFolder}{hash}.png") == false) return;

        byte[] imageData = File.ReadAllBytes($"{spriteFolder}{hash}.png");

        AddSpriteLocally(imageData, hash);
        imageCollection.AddImage(imageData, hash);
        RpcClientOnNetworkSprite(new NetworkSprite(hash));

    }
}
