using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIJoiningAndHostingPanel : MonoBehaviour {

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

    public void StartHostClicked() {
        manager.StartHost();
        StartCoroutine(connectingCoroutine());
    }

    public void StartServerClicked() {
        manager.StartServer();
        StartCoroutine(connectingCoroutine());

    }

    public void DirectConnectClicked() {
        manager.StartClient();
        StartCoroutine(connectingCoroutine());

    }

    public void IPAddressEditted(string text) {
        manager.networkAddress = text;
    }

    IEnumerator connectingCoroutine() {
        
        connectionButtons.SetActive(false);
        connectingStatus.SetActive(true);

        while (!NetworkClient.isConnected && !NetworkServer.active) {

            if (!NetworkClient.active) {
                connectionButtons.SetActive(true);
                connectingStatus.SetActive(false);
                yield break;
            }

            yield return null;
        }

        connectingStatus.SetActive(false);
        pregameUI.SetActive(false);
        gameUI.SetActive(true);

    }

}
