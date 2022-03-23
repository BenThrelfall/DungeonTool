using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UIAddTokenButton : MonoBehaviour {

    [SerializeField]
    TokenCollection tokenCollection;

    const string DebugPath = @"";

    public void OnClick() {

        var data = File.ReadAllBytes(DebugPath);
        tokenCollection.AddToken(data);

    }

}
