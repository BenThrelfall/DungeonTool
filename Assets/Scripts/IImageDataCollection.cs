using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IImageDataCollection {

    void AddImage(byte[] imageData, string hash);
    byte[] GetImage(string hash);


}