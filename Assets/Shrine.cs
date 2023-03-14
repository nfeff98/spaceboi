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
