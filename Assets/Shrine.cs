using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrine : MonoBehaviour
{
    public SpaceBoyController player;
    public Animator anim;
    private Material m;
    public Renderer renderer;
    public Color emissionColor;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void Activate()
    {
        // startup, lerp anim speed to from 0ish to 1
        anim.SetBool("activated", true);
        m = new Material(renderer.materials[0]);
        //color not updating for some reason
        m.SetColor("_EmissionColor", emissionColor);

        renderer.materials[0] = m;
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
