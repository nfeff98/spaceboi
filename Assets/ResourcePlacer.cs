using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ResourcePlacer : MonoBehaviour
{
    public RenderTexture resourceMap;
    public RenderTexture distroMap;
    public RenderTexture grassMap;
    public Material resourceMat;
    public Material distroMat;

    private Texture2D resourceTex;
    private Texture2D distroTex;
    private Texture2D grassTex;
    private int width = 1024;
    private int height = 1024;
    public Transform origin;
    private GameObject folder;

    public Planet planet;

    [Tooltip("Hardcoded to ground obj scale")]
    private float scaleFactor = 11;
    private float resolution = 1024; //should equal renderTex resolution
    public int step = 20;

    public float resourceAbundance = 30f;
    public float clusterSize = 3f;
    public float threshold = 1.5f; // sum of rgb must be higher than this in distro map

    private List<GameObject> resources = new List<GameObject>();

    [Header("Zaza Prefabs")]
    public List<GameObject> zazas = new List<GameObject>();

    [Header("Womp Prefabs")]
    public List<GameObject> womps = new List<GameObject>();

    [Header("Stromg Prefabs")]
    public List<GameObject> stromgs = new List<GameObject>();


    public GameObject grassPrefab;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroyOldResources()
    {
       
        resources.Clear();
        if (folder != null)
        {
            DestroyImmediate(folder, false);
        }
        if (GameObject.Find("__generated env"))
        {
            Destroy(GameObject.Find("__generated env"));
        }
    }

    public void PlaceNewResources()
    {
        StartCoroutine(PlaceResourceRoutine());
    }

    public IEnumerator PlaceResourceRoutine()
    {
        if (planet == null) planet = this.GetComponent<Planet>();
        RollParams();
        yield return new WaitForSeconds(0.1f);
        PlaceResources();
    }

    public void RollParams()
    {
        //randomize 
        distroMat.SetFloat("_DistributionSeed", Random.Range(30, 180));
        distroMat.SetFloat("_ResourceDensity", resourceAbundance);
        resourceMat.SetFloat("_ClusterSize", 3);
        resourceMat.SetFloat("_PlacementSeed", Random.Range(0, 100));
        ReadMap();
    }

    public void PlaceResources()
    {
        // add folders per resource
        DestroyOldResources();
        planet.totalElysium = 0;
        planet.totalStromg = 0;
        planet.totalWomp = 0;
        planet.totalWooter = 0;
        planet.totalZaza = 0;

        folder = new GameObject("__generated env");
        folder.transform.parent = origin;
        ReadMap();
        for (int y = 0; y < resolution; y += step)
        {
            for (int x = 0; x < resolution; x += step)
            {
                Color grassVal = grassTex.GetPixel(x, y);
                float sum = grassVal.g + grassVal.r + grassVal.b;
                float scalar = scaleFactor * 10f;
                Vector3 placerCast = new Vector3(x * scalar / resolution, 0, y * scalar / resolution) + origin.position;
                Vector3 resourcePos = placerCast;
                Color val2 = distroTex.GetPixel(x, y); // could use more probability here


                if (sum > 1f)
                {
                    int probability = Random.Range(1, 10);
                    if (probability <= 3)
                    {
                        
                        GameObject grass = Instantiate(grassPrefab);
                        grass.transform.parent = folder.transform;
                        grass.transform.Rotate(Vector3.up, Random.Range(0, 180));
                        Ray ray2 = new Ray(placerCast, Vector3.down);
                        RaycastHit hit2;
                        if (Physics.Raycast(ray2, out hit2, 20f))
                        {
                            if (hit2.collider.gameObject.tag == "Navigation")
                            {
                                grass.transform.position = new Vector3(placerCast.x, hit2.point.y, placerCast.z);
                                Vector2 offset = Random.insideUnitCircle;
                                //grass.transform.position += new Vector3(offset.x, 0, offset.y);
                            }
                        }
                    }
                }


                if (val2.r + val2.g + val2.b > threshold)
                {
                    //read resource map and get color
                    Color val = resourceTex.GetPixel(x, y);
                    //Debug.Log(val);
                    List<GameObject> possiblePrefabs = new List<GameObject>();
                    Inventory.Resource type = Inventory.Resource.Elysium;
                    if (val.r >= val.g && val.r >= val.b)
                    {
                        type = Inventory.Resource.Womp;
                        possiblePrefabs = womps;
                    }
                    else if (val.g >= val.r && val.g >= val.b)
                    {
                        type = Inventory.Resource.Zaza;
                        possiblePrefabs = zazas;

                    }
                    else if (val.b >= val.r && val.b >= val.g)
                    {
                        type = Inventory.Resource.Stromg;
                        possiblePrefabs = stromgs;
                    }

                    Ray ray = new Ray(placerCast, Vector3.down);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 20f))
                    {
                        if (hit.collider.gameObject.tag == "Navigation")
                        {
                            int index = Random.Range(0, possiblePrefabs.Count);
                            resourcePos.y = hit.point.y;
                            GameObject resource = Instantiate(possiblePrefabs[index]);
                            resources.Add(resource);
                            planet.AddResource(type);
                            resource.transform.position = resourcePos;
                            resource.transform.parent = folder.transform;
                            resource.transform.Rotate(Vector3.up, Random.Range(0, 180));
                        }
                        else
                        {
                            //Debug.Log("hit river");
                        }
                    }
                  
                }
                

            }
        }
    }

    public void ResetTotals()
    {
        planet.totalElysiumCreated = 0;
        planet.totalStromgCreated = 0;
        planet.totalWompCreated = 0;
        planet.totalWooterCreated = 0;
        planet.totalZazaCreated = 0;
    }

    public void ReadMap()
    {
        width = 1024;
        height = 1024;
        resourceTex = new Texture2D(width, height, TextureFormat.RGB24, false);
        distroTex = new Texture2D(width, height, TextureFormat.RGB24, false);
        grassTex = new Texture2D(width, height, TextureFormat.RGB24, false);
        Rect reader = new Rect(0, 0, width, height);
        RenderTexture.active = resourceMap;
        resourceTex.ReadPixels(reader, 0, 0); 
        resourceTex.Apply();
        RenderTexture.active = distroMap;
        distroTex.ReadPixels(reader, 0, 0);
        distroTex.Apply();

        RenderTexture.active = grassMap;
        grassTex.ReadPixels(reader, 0, 0);
        grassTex.Apply();
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(ResourcePlacer))]
    public class ResourcePlacerEditor : Editor
    {


        public override void OnInspectorGUI()
        {

            ResourcePlacer resourcePlacer = (ResourcePlacer)target;
            base.OnInspectorGUI();
            if (GUILayout.Button("RerollParams"))
            {
                resourcePlacer.RollParams();
            }

            if (GUILayout.Button("Place Resources"))
            {
                resourcePlacer.PlaceResources();
            }

            if (GUILayout.Button("Reset Totals"))
            {
                resourcePlacer.ResetTotals();
            }
        }
    }
#endif
}
