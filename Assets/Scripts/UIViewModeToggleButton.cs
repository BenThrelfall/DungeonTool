using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class UIViewModeToggleButton : MonoBehaviour {

    [SerializeField]
    RenderPipelineAsset dmRenderAsset;

    [SerializeField]
    RenderPipelineAsset playerRenderAsset;

    bool viewingPlayerView = false;

    public void OnClick() {
        GraphicsSettings.renderPipelineAsset = viewingPlayerView ? dmRenderAsset : playerRenderAsset;
        viewingPlayerView = !viewingPlayerView;
    }


}
