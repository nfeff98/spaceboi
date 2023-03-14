using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerNextLevel : MonoBehaviour {

    [SerializeField] private GameObject TriggerScreen;

    private void OnTriggerEnter(Collider other) {
        TriggerScreen.SetActive(true);
    }


    private void OnTriggerExit(Collider other) {
        TriggerScreen.SetActive(false);
    }
}
