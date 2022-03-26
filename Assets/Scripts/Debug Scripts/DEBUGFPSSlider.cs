using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DEBUGFPSSlider : MonoBehaviour {

    [SerializeField]
    TextMeshProUGUI text;

    public void OnChange(float value) {

        int intValue = Mathf.RoundToInt(value);

        text.text = $"{value} : {intValue}";
        Application.targetFrameRate = intValue;
    }

}
