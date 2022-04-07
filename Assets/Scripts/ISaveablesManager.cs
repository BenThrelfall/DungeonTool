using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveablesManager {

    IEnumerable<SaveData> GetActiveSaveData();
    void LoadFromSaveData(IEnumerable<SaveData> saveables);

}
