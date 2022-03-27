using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DungTool {
    Select,
    Pointer,
    Ruler,
}

public interface IToolManager {

    void SwitchToTool(DungTool tool);
    void DisableAllTools();
    void EnableLastTool();

}
