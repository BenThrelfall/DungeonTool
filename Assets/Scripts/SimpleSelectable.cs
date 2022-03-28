using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSelectable : NetworkBehaviour, ISelectable {
    public Bounds ObjectBounds => col.bounds;

    Collider2D col;
    Vector2 officalPos;

    private void Start() {
        col = GetComponent<Collider2D>();
        officalPos = transform.position;
    }

    public void Delete() {
        CmdDelete();
    }

    [Command(requiresAuthority = false)]
    void CmdDelete() {
        NetworkServer.Destroy(gameObject);
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
        transform.localScale = newSize;
    }

    public void Resize(Vector2 newSize) {
        transform.localScale = newSize;

    }

    [Command(requiresAuthority = false)]
    void CmdResize(Vector2 newSize) {
        transform.localScale = newSize;
        RpcResize(newSize);
    }

    [ClientRpc]
    void RpcResize(Vector2 newSize) {
        transform.localScale = newSize;
    }



}
