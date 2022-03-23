using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UIAddMapButton : MonoBehaviour {

    [SerializeField]
    MapsCollection mapCollection;

    const string DebugPath = @"";

    public void OnClick() {

        var data = File.ReadAllBytes(DebugPath);
        mapCollection.AddToken(data);

    }

}
