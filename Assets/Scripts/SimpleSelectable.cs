using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behavour designed to go on most selectable objects on the board
/// Marks objects as being an <c>ISelectable</c> and implements the
/// interface methods in a simple way. Syncs changes made to itself
/// across the network.
/// </summary>
public class SimpleSelectable : NetworkBehaviour, ISelectable {
    public Bounds ObjectBounds => col.bounds;

    Collider2D col;
    Vector2 officalPos;

    bool halfSnapped;

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

    public void ResizeWithSnapping(Vector2 newSize) {

        if (newSize.x % 2 == 0) {
            if (halfSnapped) {
                DragPosition(Vector2.one * 0.5f);
                Move();
            }
        }
        else {
            DragPosition(-Vector2.one * 0.5f);
            Move();
        }

        transform.localScale = newSize;
        CmdResize(newSize);
    }


    public void ResizeNoSnapping(Vector2 newSize) {
        transform.localScale = newSize;
        CmdResize(newSize);
    }

    [Command(requiresAuthority = false)]
    void CmdResize(Vector2 newSize) {
        ServerResize(newSize);
    }

    [Server]
    public void ServerResize(Vector2 newSize) {
        transform.localScale = newSize;
        RpcResize(newSize);
    }

    [ClientRpc]
    void RpcResize(Vector2 newSize) {
        transform.localScale = newSize;
    }

}
