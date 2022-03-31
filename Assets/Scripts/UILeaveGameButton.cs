using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UILeaveGameButton : MonoBehaviour {

    [SerializeField]
    NetworkManager manager;
    

    public void LeaveGameClicked() {

        if (NetworkServer.active && NetworkClient.isConnected) { 
                manager.StopHost();
        }
        // stop client if client-only
        else if (NetworkClient.isConnected) {
                manager.StopClient();
        }
        // stop server if server-only
        else if (NetworkServer.active) {
            manager.StopServer();
        }

        Destroy(manager.gameObject);

        SceneManager.LoadScene(0);

    }

}
