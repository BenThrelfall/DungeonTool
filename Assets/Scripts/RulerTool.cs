using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RulerTool : NetworkBehaviour {

    public GameObject rulerObject;
    public Camera mainCamera;

    public float width;

    public float fiveEDistance;
    public float realDistance;

    private void Start() {
        if (mainCamera == null) mainCamera = Camera.main;
    }

    public void PlaceRulerTool() {
        if (Input.GetMouseButtonDown(0)) {
            Vector2 rawStartPoint = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 startPoint = new Vector2(Mathf.RoundToInt(rawStartPoint.x), Mathf.RoundToInt(rawStartPoint.y));
            CmdSpawnRuler(startPoint);
        }
    }

    IEnumerator PlaceRuler(Vector2 startPoint, GameObject ruler) {

        Vector2 rawCurrentPos;
        Vector2 currentPos;

        var rulerComp = ruler.GetComponent<Ruler>();

        while (Input.GetMouseButton(0)) {
            rawCurrentPos = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);
            currentPos = new Vector2(Mathf.RoundToInt(rawCurrentPos.x), Mathf.RoundToInt(rawCurrentPos.y));

            rulerComp.SetRuler(startPoint, currentPos, width);

            yield return null;
        }

        CmdDestroyRuler(ruler);
    }

    [Command(requiresAuthority = false)]
    private void CmdDestroyRuler(GameObject ruler) {
        NetworkServer.Destroy(ruler);
    }

    [Command(requiresAuthority = false)]
    void CmdSpawnRuler(Vector2 startPoint, NetworkConnectionToClient conn = null) {
        GameObject ruler = Instantiate(rulerObject, startPoint, Quaternion.identity);
        NetworkServer.Spawn(ruler);
        RulerSpawned(conn, startPoint, ruler);
    }

    [TargetRpc]
    private void RulerSpawned(NetworkConnection conn, Vector2 startPoint, GameObject ruler) {
        StartCoroutine(PlaceRuler(startPoint, ruler));
    }

}

