using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFileIOService {

    void ReadAllBytes(Action<byte[]> action);

}
