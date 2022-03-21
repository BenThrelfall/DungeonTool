using Mirror;
using Mirror.SimpleWeb;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebTestNetworkManager : NetworkManager {

    public override void Awake() {
        var webTransport = gameObject.AddComponent<SimpleWebTransport>();
        transport = webTransport;

        autoCreatePlayer = false;

        base.Awake();

    }

}
