using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILeaveGameButton : MonoBehaviour {

    [SerializeField]
    NetworkManager manager;

    [SerializeField]
    GameObject pregameUI;

    [SerializeField]
    GameObject connectionButtons;

    [SerializeField]
    GameObject connectingStatus;

    [SerializeField]
    GameObject gameUI;

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

        pregameUI.SetActive(true);
        connectionButtons.SetActive(true);
        connectingStatus.SetActive(false);

        gameUI.SetActive(false);

    }

}
