using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public GameObject outline;
    public Transform interactPos;
    public enum InteractionType { Mining };
    public InteractionType type;
    // Start is called before the first frame update
    void Start()
    {
        if (outline == null)
        {
            outline = this.transform.Find("Outline").gameObject;
            outline.SetActive(false);
        }

        if (interactPos == null)
        {
            interactPos = this.transform.Find("InteractPos").transform;
        }
    }

    public void EnableHighlight()
    {
        outline.SetActive(true);
        Debug.Log("hit me!");
    }

    public void DisableHighlight()
    {
        outline.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Capsule")
            Debug.Log("entered");
    }
}
