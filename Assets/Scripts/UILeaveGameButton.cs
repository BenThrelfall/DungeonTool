using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class UILeaveGameButton : MonoBehaviour {

    [SerializeField]
    NetworkManager manager;

    [SerializeField]
    RenderPipelineAsset defaultRenderAsset;

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

        //Destroy network manager because a new one is created when the scene is reloaded
        Destroy(manager.gameObject);

        //Reset renderpipeline
        GraphicsSettings.renderPipelineAsset = defaultRenderAsset;

        //Reload scene
        SceneManager.LoadScene(0);

    }

}
