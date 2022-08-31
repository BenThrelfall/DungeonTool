
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveComp {

    public CompSaveData Save();

    public void Load(CompSaveData data);

}