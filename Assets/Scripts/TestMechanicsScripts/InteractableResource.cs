using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableResource : MonoBehaviour {


    [SerializeField] GameObject resource;
    [SerializeField] Inventory.Resource type;
    [SerializeField] SpaceBoyController spaceBoy;

    private int health;
    [SerializeField] private int minHealth = 2;
    [SerializeField] private int maxHealth = 4;

    private float xPos;
    private float zPos;
    [SerializeField] private float minPos = 1f;
    [SerializeField] private float maxPos = 3f;
    private ParticleSystem hitpfx;
    public  ParticleSystem hitpfxPrefab;

    public Planet planet;

    private void Awake() {
        health = Random.Range(minHealth, maxHealth + 1);
        hitpfx = Instantiate(hitpfxPrefab);
        hitpfx.transform.SetParent(transform);
        hitpfx.transform.localPosition = Vector3.zero;
        planet = FindObjectOfType<Planet>();
        type = resource.GetComponent<Resource>().ResourceType;
    }


    public void Interact() {
        StartCoroutine(InteractRoutine());
    }



    public IEnumerator InteractRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        UpdateHealth();
        DropResource();
        yield return new WaitForSeconds(0.2f);
        Vector3 toSpaceboy = spaceBoy.transform.position - this.transform.position;
        toSpaceboy += new Vector3(0.75f, 0);
        hitpfx.transform.localPosition = new Vector3(0, 0.75f, 0) + toSpaceboy * 0.01f;
        hitpfx.transform.forward = toSpaceboy;
        hitpfx.Play();

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


    private void DropResource() {
        Debug.Log("Resource Dropped!");
        xPos = Random.Range(minPos, maxPos);
        zPos = Random.Range(minPos, maxPos);
        //Vector3 pos = new Vector3(xPos + spaceBoy.transform.localPosition.x, 2f, zPos + spaceBoy.transform.localPosition.z);
        Vector2 outPos = Random.insideUnitCircle * 2f;
        GameObject g = Instantiate(resource, this.transform.position + new Vector3(outPos.x, 5f, outPos.y), Quaternion.identity);
        Vector3 outDir = Random.insideUnitSphere;
        outDir.y = Mathf.Abs(outDir.y);
        g.GetComponent<Rigidbody>().AddForce(outDir);
    }
}
