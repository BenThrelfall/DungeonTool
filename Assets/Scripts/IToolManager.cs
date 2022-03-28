using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DungTool {
    Select,
    Pointer,
    Ruler,
}

public interface IToolManager {

    event Action<DungTool> ToolChanged;

    void SwitchToTool(DungTool tool);
    void DisableAllTools();
    void EnableLastTool();

}
