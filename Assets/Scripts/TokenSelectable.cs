using Mirror;
using Newtonsoft.Json;
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

    private void Start() {
        officalPos = transform.position;
        SetUpDependancies(DependancyInjector.instance.Services);
    }

    public override void OnStartClient() {
        base.OnStartClient();

        CmdFetchScale();
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
    public void CmdFetchScale(NetworkConnectionToClient sender = null) {
        TargetSetScale(sender, spriteTrans.localScale);
    }

    [TargetRpc]
    public void TargetSetScale(NetworkConnection target, Vector3 scale) {
        spriteTrans.localScale = scale;
        col.size = scale;
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
            Json = JsonConvert.SerializeObject(spriteTrans.localScale)
        };
    }

    public void Load(CompSaveData data) {
        Vector3 size = JsonConvert.DeserializeObject<Vector3>(data.Json);
        spriteTrans.localScale = size;
        col.size = size;
        
    }
}

