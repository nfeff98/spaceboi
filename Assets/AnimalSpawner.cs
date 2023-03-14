using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public ResourcePlacer rp;
    private int resolution = 1024;
    public GameObject[] animalPrefabs;
    public int[] packSizes;
    public int[] frequencyProbability;
    private GameObject folder;
    void Start()
    {

        //PlaceAnimals();
    }

    public void PlaceAnimals()
    {
        folder = new GameObject("animalFolder");
        //folder.transform.parent = rp.folder.transform;
        for (int i = 0; i < animalPrefabs.Length; i++) {
            for (int y = 0; y < resolution; y += rp.step)
            {
                for (int x = 0; x < resolution; x += rp.step)
                {
                    if (Random.Range(0, frequencyProbability[i]) == 1)
                    {
                        Debug.Log("placing animals");

                        float scalar = 11 * 10f;
                        Vector3 placerCast = new Vector3(x * scalar / resolution, 0, y * scalar / resolution) + rp.origin.position;
                        Ray ray = new Ray(placerCast, Vector3.down);
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit, 20f))
                        {
                            if (hit.collider.gameObject.tag == "Navigation" || hit.collider.gameObject.name.Contains("rass"))
                            {
                                int packSize = Random.Range(1, packSizes[i]);
                                for (int j = 0; j < packSize; j++)
                                {
                                    GameObject animal = Instantiate(animalPrefabs[i]);
                                    animal.transform.position = hit.point;
                                    animal.transform.parent = folder.transform;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
