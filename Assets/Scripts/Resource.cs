using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    // Start is called before the first frame update

    public Inventory.Resource ResourceType;
    private Inventory inv;
    private bool grounded;
    public Tutorial tutorial;
    void Start()
    {
        inv = FindObjectOfType<Inventory>();
        
    }

   private void OnTriggerEnter(Collider other)
    {
       // Debug.Log("We collided");
        if (other.gameObject.tag == "Player" && grounded)
        {
            if (inv != null)
            {
                inv.AddToInventory(ResourceType);
                Debug.Log("picked up " + ResourceType.ToString());
                Destroy(this.gameObject);
                
                // If zaza is grabbed for the first time play eatZazaTutorial
                /*if (!Tutorial.zazaPickedUp && !DialogueManager.GetInstance().dialogueIsPlaying) {
                    DialogueManager.GetInstance().EnterDialogueMode(Tutorial.eatZazaTutorial);
                    Tutorial.zazaPickedUp = true;
                }
                */
            }
        }

        if (other.gameObject.tag == "Navigation")
        {
            this.GetComponent<Rigidbody>().isKinematic = true;
            grounded = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
