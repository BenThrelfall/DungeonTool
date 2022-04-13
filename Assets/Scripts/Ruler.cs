using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

/// <summary>
/// Behavour for the ruler object created by the <c>RulerTool</c>.
/// Has a public method for updating the rulers position and then 
/// calculates new values and syncs across the server.
/// </summary>
public class Ruler : NetworkBehaviour {

    [SerializeField]
    SpriteRenderer spriteRenderer;

    [SerializeField]
    Transform line;

    [SerializeField]
    TextMeshPro text;

    public void SetRuler(Vector2 startPos, Vector2 currentPos, float width) {
        UpdateRuler(startPos, currentPos, width);
        CmdUpdatePos(startPos, currentPos, width);
    }

    private void UpdateRuler(Vector2 startPos, Vector2 currentPos, float width) {
        var dif = currentPos - startPos;
        line.up = dif;
        var scale = new Vector3(width, dif.magnitude, 1);
        var position = startPos + dif / 2;

        spriteRenderer.size = scale;
        transform.position = position;

        var fiveEDistance = Mathf.Abs(dif.x) > Mathf.Abs(dif.y) ? Mathf.Abs(dif.x) : Mathf.Abs(dif.y);
        fiveEDistance *= 5;
        //var realDistance = dif.magnitude;

        text.text = fiveEDistance.ToString();
        RpcUpdateNumber(fiveEDistance.ToString(), scale);
    }

    [Command(requiresAuthority = false)]
    void CmdUpdatePos(Vector2 startPos, Vector2 currentPos, float width) {
        UpdateRuler(startPos, currentPos, width);
    }

    [ClientRpc]
    void RpcUpdateNumber(string number, Vector2 scale) {
        text.text = number;
        spriteRenderer.size = scale;
    }

}
