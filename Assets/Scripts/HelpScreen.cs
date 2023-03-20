using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpScreen : MonoBehaviour {

    [SerializeField] private GameObject helpScreen;
    [SerializeField] private GameObject quotaScreen;

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.H)) {
            ActivateHelpScreen();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ActivateQuotaScreen();
        }
    }

    public void ActivateHelpScreen() {
        helpScreen.SetActive(!helpScreen.activeInHierarchy);
    }


    public void ActivateQuotaScreen() {
        quotaScreen.SetActive(!quotaScreen.activeInHierarchy);
    }
}
