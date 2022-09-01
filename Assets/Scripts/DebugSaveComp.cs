using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSaveComp : MonoBehaviour, ISaveComp {
    public CompType ComponentType => CompType.DebugSaveComp;

    public void Load(CompSaveData data) {
        return;
    }

    public CompSaveData Save() {
        return new CompSaveData(ComponentType);
    }

}
