using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleRulerTool : NetworkBehaviour {

    public GameObject rulerObject;
    public Camera mainCamera;

    GameObject activeRuler;

    private void Start() {
        if (mainCamera == null) mainCamera = Camera.main;
    }

    /// <summary>
    /// Main method
    /// </summary>
    public void PlaceRulerTool() {
        if (Input.GetMouseButtonDown(0)) {
            if (activeRuler != null) CmdDestroyRuler(activeRuler);
            Vector2 rawStartPoint = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 startPoint = new Vector2(Mathf.RoundToInt(rawStartPoint.x - 0.5f) + 0.5f, Mathf.RoundToInt(rawStartPoint.y - 0.5f) + 0.5f);
            CmdSpawnRuler(startPoint);
        }
    }

    /// <summary>
    /// Updates the position of the ruler while the mouse button is being held then calls
    /// the command to destroy the ruler when the button is released.
    /// </summary>
    /// <param name="startPoint">Location being measured from</param>
    /// <param name="ruler">Reference to the ruler object</param>
    /// <returns></returns>
    IEnumerator PlaceRuler(Vector2 startPoint, GameObject ruler) {

        Vector2 rawCurrentPos;
        Vector2 currentPos;

        var rulerComp = ruler.GetComponent<CircleRuler>();

        while (Input.GetMouseButton(0)) {
            rawCurrentPos = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);
            currentPos = new Vector2(Mathf.RoundToInt(rawCurrentPos.x - 0.5f) + 0.5f, Mathf.RoundToInt(rawCurrentPos.y - 0.5f)  + 0.5f);

            rulerComp.SetRuler(startPoint, currentPos);

            yield return null;
        }
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
        activeRuler = ruler;
        StartCoroutine(PlaceRuler(startPoint, ruler));
    }

}
