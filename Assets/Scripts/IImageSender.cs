using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IImageSender {

    void SendToServer(byte[] imageData, string hash);
    void SendToAllClients(byte[] imageData, string hash);

}
