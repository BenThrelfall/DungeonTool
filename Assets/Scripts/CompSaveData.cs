using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CompType {
    SyncedRuntimeSprite,
    TokenSelectable,
    DebugSaveComp
}

public class CompSaveData {

    public CompType compType;

    public string Json { get; set; }

    public CompSaveData(CompType compType) {
        this.compType = compType;
    }

}