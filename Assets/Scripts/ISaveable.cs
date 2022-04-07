using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IObjectSpawner;
using System;

public interface ISaveable {
    public SaveData SaveData { get; }
}
