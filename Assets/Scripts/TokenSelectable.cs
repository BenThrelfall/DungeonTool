using Mirror;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenSelectable : NetworkBehaviour, ISelectable, IRequiresDependancy, ISaveComp {
    public Bounds ObjectBounds => col.bounds;
    public CompType ComponentType => CompType.TokenSelectable;

    [SerializeField]
    BoxCollider2D col;
    Vector2 officalPos;
    
    [SerializeField]
    Transform spriteTrans;

    IObjectSpawner spawner;

    [SerializeField]
    VisionPerciever lightComp;

    private void Start() {
        officalPos = transform.position;
        SetUpDependancies(DependancyInjector.instance.Services);
    }

    public override void OnStartClient() {
        base.OnStartClient();

        CmdInitalSync();
    }

    public void Delete() {
        spawner.DespawnObject(gameObject);
    }

    public void DragPosition(Vector2 delta) {
        transform.position = (Vector3)officalPos + (Vector3)delta;
    }

    public void Move() {
        CmdMove(transform.position);
    }

    [Command(requiresAuthority = false)]
    void CmdMove(Vector2 newPos) {
        transform.position = newPos;
        officalPos = newPos;
        RpcMove(newPos);
    }

    [ClientRpc]
    void RpcMove(Vector2 newPos) {
        officalPos = newPos;
        transform.position = newPos;
    }

    public void DragSize(Vector2 newSize) {
        spriteTrans.localScale = newSize;
        col.size = newSize;
    }

    public void ResizeWithSnapping(Vector2 newSize) {

        if (newSize.x > spriteTrans.localScale.x) {
            DragPosition(Vector2.one * 0.5f);
            Move();
        }
        else {
            DragPosition(-Vector2.one * 0.5f);
            Move();
        }

        spriteTrans.localScale = newSize;
        col.size = newSize;
        CmdResize(newSize);
    }


    public void ResizeNoSnapping(Vector2 newSize) {
        spriteTrans.localScale = newSize;
        col.size = newSize;
        CmdResize(newSize);
    }

    [Command(requiresAuthority = false)]
    void CmdResize(Vector2 newSize) {
        ServerResize(newSize);
    }

    [Server]
    public void ServerResize(Vector2 newSize) {
        spriteTrans.localScale = newSize;
        col.size = newSize;
        RpcResize(newSize);
    }

    [Command(requiresAuthority = false)]
    public void CmdInitalSync(NetworkConnectionToClient sender = null) {
        TargetInitalSync(sender, spriteTrans.localScale, lightComp.viewDistance);
    }

    [TargetRpc]
    public void TargetInitalSync(NetworkConnection target, Vector3 scale, float viewDistance) {
        spriteTrans.localScale = scale;
        col.size = scale;
        lightComp.viewDistance = viewDistance;
    }

    [ClientRpc]
    void RpcResize(Vector2 newSize) {
        spriteTrans.localScale = newSize;
        col.size = newSize;
    }

    public void SetUpDependancies(ServiceCollection serviceCollection) {
        spawner = serviceCollection.GetService<IObjectSpawner>();
    }

    public CompSaveData Save() {
        return new CompSaveData(ComponentType) {
            Json = JsonConvert.SerializeObject((spriteTrans.localScale, lightComp.viewDistance))
        };
    }

    public void Load(CompSaveData data) {
        (Vector3, float) deserializedData = JsonConvert.DeserializeObject<(Vector3, float)>(data.Json);
        spriteTrans.localScale = deserializedData.Item1;
        col.size = deserializedData.Item1;
        lightComp.viewDistance = deserializedData.Item2;
        RpcSetLightSize(deserializedData.Item2);
    }

    [ClientRpc]
    private void RpcSetLightSize(float distance) {
        lightComp.viewDistance = distance;
    }

    public void IncreaseLight() {
        CmdChangeLightDistance(1);
    }

    public void DecreaseLight() {
        CmdChangeLightDistance(-1);
    }

    [Command(requiresAuthority = false)]
    void CmdChangeLightDistance(float delta) {
        RpcChangeLightDistance(delta);
    }

    [ClientRpc]
    void RpcChangeLightDistance(float delta) {
        lightComp.ChangeViewDistance(delta);
    }
}

