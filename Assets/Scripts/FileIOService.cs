using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleFileBrowser;
using System;
using System.Linq;
using System.IO;
using static SimpleFileBrowser.FileBrowser;

/// <summary>
/// Implementation of <c>IFileIOService</c> that uses <c>SimpleFileBrowser</c>
/// </summary>
public class FileIOService : MonoBehaviour, IFileIOService {

    public void ReadAllBytes(Action<byte[]> action) {
        FileBrowser.ShowLoadDialog(
                onSuccess: (x) => {
                    action(File.ReadAllBytes(x.First()));
                },
                onCancel: () => { },
                PickMode.Files
            );
    }

}
