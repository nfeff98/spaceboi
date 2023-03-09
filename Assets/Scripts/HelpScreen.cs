using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpScreen : MonoBehaviour {

    [SerializeField] private GameObject helpScreen;

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.H)) {
            helpScreen.SetActive(!helpScreen.activeInHierarchy);
        }
    }
}
