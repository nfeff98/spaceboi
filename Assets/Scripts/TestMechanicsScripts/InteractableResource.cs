using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableResource : MonoBehaviour {


    [SerializeField] GameObject resource;
    [SerializeField] SpaceBoyController spaceBoy;

    private int health;
    [SerializeField] private int minHealth = 2;
    [SerializeField] private int maxHealth = 4;

    private float xPos;
    private float zPos;
    [SerializeField] private float minPos = 1f;
    [SerializeField] private float maxPos = 3f;


    private void Awake() {
        health = Random.Range(minHealth, maxHealth + 1);
    }


    public void Interact() {
        UpdateHealth();
        DropResource();
    }


    private void UpdateHealth() {
        if (health > 0) {
            health -= 1;
            Debug.Log("Health: " + health);
            if (health == 0) {
                Destroy(this.gameObject);
                Debug.Log("Resource Extracted!");
            }
        }
    }


    private void DropResource() {
        Debug.Log("Resource Dropped!");
        xPos = Random.Range(minPos, maxPos);
        zPos = Random.Range(minPos, maxPos);
        Vector3 pos = new Vector3(xPos + spaceBoy.transform.localPosition.x, 2f, zPos + spaceBoy.transform.localPosition.z);
        Instantiate(resource, pos, Quaternion.identity);
    }
}
