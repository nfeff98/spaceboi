using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrine : MonoBehaviour
{
    public SpaceBoyController player;
    public Animator anim;
    public Material m;
    public Renderer[] renderers;
    public Color emissionColor;
    public GameObject ribbonPFX;
    public AnimalBehavior[] animals;
    bool updated = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void Activate()
    {
        // startup, lerp anim speed to from 0ish to 1
        anim.SetBool("activated", true);
       // m.SetColor("_EmissionColor", emissionColor);
        foreach (Renderer r in renderers)
        {
            //color not updating for some reason
            r.material = m;
          
        }
        ribbonPFX.SetActive(true);
        animals = FindObjectsOfType<AnimalBehavior>();
        foreach (AnimalBehavior animal in animals)
        {
            if (animal.type == AnimalBehavior.AnimalType.Penguin)
            {
                animal.wandering = false;
                Vector2 offset = Random.insideUnitCircle.normalized * 2;
                animal.agent.destination = this.transform.position + new Vector3(offset.x, 0, offset.y);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        //make sure shrine is in a little clearing
        if (other.GetComponent<InteractableResource>() != null)
        {
            Destroy(other.gameObject);
        }

        if (other.tag == "Player")
        {
            
        }
    }
}
