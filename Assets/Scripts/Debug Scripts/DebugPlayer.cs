using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPlayer : MonoBehaviour {

    // Update is called once per frame
    void Update() {

        if (Input.GetKey(KeyCode.UpArrow)) { 
            transform.Translate(Vector3.up * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.DownArrow)) { 
            transform.Translate(Vector3.down * Time.deltaTime);   
        }

    }

}
