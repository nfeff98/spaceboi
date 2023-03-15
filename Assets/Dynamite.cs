using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamite : MonoBehaviour
{
    // Start is called before the first frame update

    //Dynamite controller
    // goals - destroys rocks and kills penguins (fry/launch them first?) 
    // 
    //

    public SphereCollider shockwave;
    public Animator animator;
 

    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out InteractableResource r))
        {
            if (r.type == Inventory.Resource.Stromg || r.type == Inventory.Resource.Elysium)
            {
                Debug.Log("hit rock");
                //rocksInRange.Add(r);
                StartCoroutine(DestroyRock(r));
                
            }
        } 
        if (other.TryGetComponent(out AnimalBehavior animal))
        {
            //animalsInRange.Add(animal);
            Debug.Log("hit penguin");
            animal.GetComponent<Rigidbody>().AddExplosionForce(5000, this.transform.position - new Vector3(0, -2f, 0), shockwave.radius * 3f);
            animal.GetComponent<Rigidbody>().AddForce(Vector3.up * 1000);
            StartCoroutine(KillAfterLaunch(animal));
        }
    }

    public void Explosion()
    {
        StartCoroutine(ExplosionRoutine());
    }

    public IEnumerator ExplosionRoutine()
    {
        float t = 0;
        shockwave.enabled = true;
        bool done = false;
        while (!done)
        {
            shockwave.radius = Mathf.Lerp(0, 8f, t);
            t += 0.2f;
            if (t > 1) done = true;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }

    public IEnumerator KillAfterLaunch(AnimalBehavior animal)
    {
        yield return new WaitForSeconds(0.2f);
        animal.health = 0;
    }

    public IEnumerator DestroyRock(InteractableResource r)
    {
        while (r.health > 0)
        {
            r.Interact();
            yield return new WaitForEndOfFrame();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
