using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsButton : MonoBehaviour {


    [SerializeField] private GameObject controlsScreen;

    public void OpenControlsScreen() {
        controlsScreen.SetActive(!controlsScreen.activeInHierarchy);
    }
}
