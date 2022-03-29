using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DungTool {
    Select,
    Pointer,
    Ruler,
}

/// <summary>
/// Manages the tools which control what user input does in the game.
/// Switches between tools deactivating non-enabled tools
/// </summary>
public interface IToolManager {

    /// <summary>
    /// Fired when the active tool is changed.
    /// <c>DungTool</c> is what the active tool has been
    /// changed to.
    /// </summary>
    event Action<DungTool> ToolChanged;

    /// <summary>
    /// Switches to a specific tool, deactivating all other ones
    /// </summary>
    /// <param name="tool">Tool to change to</param>
    void SwitchToTool(DungTool tool);

    /// <summary>
    /// Deactivates all tools.
    /// </summary>
    void DisableAllTools();

    /// <summary>
    /// Switches the active tool to whatever tool was active before the currenlty
    /// active tool.
    /// </summary>
    void EnableLastTool();

}
