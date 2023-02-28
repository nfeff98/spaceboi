using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedInteractableResourceVisual : MonoBehaviour {


    [SerializeField] private InteractableResource interactableResource;
    [SerializeField] private GameObject visualGameObject;


    private void Start() {
        SpaceBoyController.Instance.OnSelectedResourceChanged += Player_OnSelectedResourceChanged;
    }

    private void Player_OnSelectedResourceChanged(object sender, SpaceBoyController.OnSelectedResourceChangedEventArgs e) {
        if (e.selectedResource == interactableResource) {
            Show();
        } else {
            Hide();
        }
    }

    private void Show() {
        if (visualGameObject != null) {
            visualGameObject.SetActive(true);
        }
    }


    private void Hide() {
        if (visualGameObject != null) {
            visualGameObject.SetActive(false);
        }
    }
}
