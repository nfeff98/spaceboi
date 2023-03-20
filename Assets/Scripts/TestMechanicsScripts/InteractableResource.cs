using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableResource : MonoBehaviour {


    [SerializeField] GameObject resource;
    public Inventory.Resource type;
    [SerializeField] SpaceBoyController spaceBoy;

    public int health;
    

    private float xPos;
    private float zPos;
    [SerializeField] private float minPos = 1f;
    [SerializeField] private float maxPos = 3f;
    private ParticleSystem hitpfx;
    public  ParticleSystem hitpfxPrefab;

    public Planet planet;

    private void Awake() {
        //health = Random.Range(minHealth, maxHealth + 1);
        hitpfx = Instantiate(hitpfxPrefab);
        hitpfx.transform.SetParent(transform);
        hitpfx.transform.localPosition = Vector3.zero;
        planet = FindObjectOfType<Planet>();
        type = resource.GetComponent<Resource>().ResourceType;
    }


    public void Interact() {
        UpdateHealth();
        Vector3 toSpaceboy = spaceBoy.transform.position - this.transform.position;

        if (this.type == Inventory.Resource.Zaza)
        {
            toSpaceboy += new Vector3(0, 0.75f, 0f);
            hitpfx.transform.localPosition = new Vector3(-0.260617733f, 0.0700000003f, 3.73000002f);
        }
        else
        {
            toSpaceboy += new Vector3(0, 0.75f, 0);
            hitpfx.transform.localPosition = new Vector3(0, 0.75f, 0) + toSpaceboy * 0.01f;
        }
        DropResource(hitpfx.transform.position);
        hitpfx.transform.forward = toSpaceboy;
        hitpfx.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<SimpleCarController>() != null)
        {
            SimpleCarController vehicle = other.GetComponentInParent<SimpleCarController>();
            if (vehicle.miningType == this.type)
            {
                if (vehicle.miningBits.Contains(other.gameObject) && vehicle.toolsOn)
                {
                    StartCoroutine(MiningCoroutine());
                }
            }
        }
    }

    public IEnumerator MiningCoroutine()
    {
        while (true)
        {
            this.Interact();
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<SimpleCarController>() != null)
        {
            SimpleCarController vehicle = other.GetComponentInParent<SimpleCarController>();
            if (vehicle.miningType == this.type)
            {
                if (vehicle.miningBits.Contains(other.gameObject))
                {
                    StopAllCoroutines();
                }
            }
        }
    }





    private void UpdateHealth() {
        if (health > 0) {
            health -= 1;
            Debug.Log("Health: " + health);
            if (health == 0) {
                planet.SubtractResource(type);
                Destroy(this.gameObject);
                Debug.Log("Resource Extracted!");
                
            }
        }
    }


    private void DropResource(Vector3 pos) {
        Debug.Log("Resource Dropped!");
        xPos = Random.Range(minPos, maxPos);
        zPos = Random.Range(minPos, maxPos);
        //Vector3 pos = new Vector3(xPos + spaceBoy.transform.localPosition.x, 2f, zPos + spaceBoy.transform.localPosition.z);
        Vector2 outPos = Random.insideUnitCircle * 2f;
        GameObject g = Instantiate(resource, pos, Quaternion.identity);
        g.transform.parent = this.transform.parent;
        Vector3 outDir = Random.insideUnitSphere;
        outDir.y = 1f;
        g.GetComponent<Rigidbody>().AddForce(outDir * 6f);
    }
}
