using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CircleRuler : NetworkBehaviour, ISelectable {

    [SerializeField]
    Transform circle;

    [SerializeField]
    TextMeshPro text;

    [SerializeField]
    Collider2D col;

    public Bounds ObjectBounds => col.bounds;
    Vector2 officalPos;

    private void Start() {
        officalPos = transform.position;
    }

    public void SetRuler(Vector2 startPos, Vector2 currentPos) {
        UpdateRuler(startPos, currentPos);
        CmdUpdatePos(startPos, currentPos);
    }

    private void UpdateRuler(Vector2 startPos, Vector2 currentPos) {
        var dif = currentPos - startPos;
        circle.up = dif;
        var scale = new Vector3(dif.magnitude, dif.magnitude, 1);

        circle.localScale = scale;

        var realDistance = dif.magnitude;
        realDistance *= 5;

        text.text = realDistance.ToString("#.#");
        RpcUpdateNumber(realDistance.ToString("#.#"));
    }

    [Command(requiresAuthority = false)]
    void CmdUpdatePos(Vector2 startPos, Vector2 currentPos) {
        UpdateRuler(startPos, currentPos);
    }

    [ClientRpc]
    void RpcUpdateNumber(string number) {
        text.text = number;
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
        return;
    }

    public void ResizeWithSnapping(Vector2 newSize) {
        return;
    }

    public void ResizeNoSnapping(Vector2 newSize) {
        return;
    }

    [Server]
    public void ServerResize(Vector2 _) {
        return;
    }

    public void IncreaseLight() {
        return;
    }

    public void DecreaseLight() {
        return;
    }
}
