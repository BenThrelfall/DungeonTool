using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used for reading and writting files and opening the file browser for
/// users to pick files to be read / written to.
/// </summary>
public interface IFileIOService {

    /// <summary>
    /// Prompts the user to choose a file through a browser then
    /// reads all the bytes from the file and pass them into an <c>Action</c>
    /// </summary>
    /// <param name="action"><c>Action</c> to be performed on the bytes</param>
    void ReadAllBytes(Action<byte[]> action);

}
