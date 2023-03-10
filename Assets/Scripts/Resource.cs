using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    // Start is called before the first frame update

    public Inventory.Resource ResourceType;
    private Inventory inv;
    private bool grounded;
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
