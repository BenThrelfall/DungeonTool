using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugTargetFramerateDisplay : MonoBehaviour {

    [SerializeField]
    TextMeshProUGUI text;
    
    // Start is called before the first frame update
    void Start() {
        StartCoroutine(UpdateFrameRate());
    }

    IEnumerator UpdateFrameRate() {

        while (true) {
            text.text = Application.targetFrameRate.ToString();
            yield return new WaitForSeconds(0.2f);
        }

    }

}
