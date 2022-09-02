using Mirror;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSelectable : NetworkBehaviour, ISelectable, IRequiresDependancy, ISaveComp {
    public Bounds ObjectBounds => col.bounds;
    public CompType ComponentType => CompType.LightSelectable;

    [SerializeField]
    BoxCollider2D col;
    Vector2 officalPos;

    IObjectSpawner spawner;

    [SerializeField]
    VisionPerciever lightComp;

    private void Start() {
        officalPos = transform.position;
        SetUpDependancies(DependancyInjector.instance.Services);
    }

    public override void OnStartClient() {
        base.OnStartClient();
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
        return;
    }

    public void ResizeWithSnapping(Vector2 newSize) {
        return;
    }


    public void ResizeNoSnapping(Vector2 newSize) {
        return;
    }

    [Server]
    public void ServerResize(Vector2 newSize) {
        return;
    }

    public void SetUpDependancies(ServiceCollection serviceCollection) {
        spawner = serviceCollection.GetService<IObjectSpawner>();
    }

    public CompSaveData Save() {
        return new CompSaveData(ComponentType) { Json = "" };
    }

    public void Load(CompSaveData data) {
        return;
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

