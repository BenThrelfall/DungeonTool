using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenSelectable : NetworkBehaviour, ISelectable, IRequiresDependancy {
    public Bounds ObjectBounds => col.bounds;

    BoxCollider2D col;
    Vector2 officalPos;
    
    [SerializeField]
    Transform spriteTrans;

    IObjectSpawner spawner;

    private void Start() {
        col = GetComponent<BoxCollider2D>();
        officalPos = transform.position;
        SetUpDependancies(DependancyInjector.instance.Services);
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

    public void Resize(Vector2 newSize) {

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

    [Command(requiresAuthority = false)]
    void CmdResize(Vector2 newSize) {
        spriteTrans.localScale = newSize;
        col.size = newSize;
        RpcResize(newSize);
    }

    [ClientRpc]
    void RpcResize(Vector2 newSize) {
        spriteTrans.localScale = newSize;
        col.size = newSize;
    }

    public void SetUpDependancies(ServiceCollection serviceCollection) {
        spawner = serviceCollection.GetService<IObjectSpawner>();
    }
}

