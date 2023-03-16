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
    public GameObject folder;
    public GameObject shrinePrefab;

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

    private void Awake()
    {
        folder = GameObject.Find("__generated env");
    }
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

    public void PlaceNewResources(float distroMatFloat, float resourceMatFloat)
    {
        StartCoroutine(PlaceResourceRoutine(distroMatFloat, resourceMatFloat));
    }

    public IEnumerator PlaceResourceRoutine(float distroMatFloat, float resourceMatFloat)
    {
        if (planet == null) planet = this.GetComponent<Planet>();
        RollParams(distroMatFloat, resourceMatFloat);
        yield return new WaitForSeconds(0.1f);
        PlaceResources();
    }

    public void RollParams(float distroMatFloat, float resourceMatFloat)
    {
        //randomize 
        distroMat.SetFloat("_DistributionSeed", distroMatFloat);
        distroMat.SetFloat("_ResourceDensity", resourceAbundance);
        resourceMat.SetFloat("_ClusterSize", 3);
        resourceMat.SetFloat("_PlacementSeed", resourceMatFloat);
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
        bool placedShrine = false;
        folder = new GameObject("__generated env");
        GameObject grassFolder = new GameObject("grassFolder");
        grassFolder.transform.parent = folder.transform;
        GameObject rockFolder = new GameObject("rockFolder");
        rockFolder.transform.parent = folder.transform;
        GameObject treeFolder = new GameObject("treeFolder");
        treeFolder.transform.parent = folder.transform;
        GameObject zazaFolder = new GameObject("zazaFolder");
        zazaFolder.transform.parent = folder.transform;
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
                    Ray ray2 = new Ray(placerCast, Vector3.down);
                    RaycastHit hit2;
                    if (Physics.Raycast(ray2, out hit2, 20f))
                    {
                        if (hit2.collider.gameObject.tag == "Navigation") // make sure we aren't in the river
                        {
                            int probability = Random.Range(1, 10);
                            if (probability <= 3)
                            {

                                GameObject grass = Instantiate(grassPrefab);
                                grass.transform.parent = grassFolder.transform;
                                grass.transform.Rotate(Vector3.up, Random.Range(0, 180));


                                grass.transform.position = new Vector3(placerCast.x, hit2.point.y, placerCast.z);

                                //grass.transform.position += new Vector3(offset.x, 0, offset.y);


                            }
                            int probability2 = Random.Range(1, 1000);
                            if (probability2 == 1 && !placedShrine && (x > 50) && (y > 50))
                            {
                                placedShrine = true;
                                Vector2 offset = Random.insideUnitCircle;
                                GameObject shrine = Instantiate(shrinePrefab);
                                shrine.transform.parent = folder.transform;
                                shrine.transform.localEulerAngles = new Vector3(0, Random.Range(-50f, 50f), 0);
                                shrine.transform.position = new Vector3(placerCast.x, hit2.point.y , placerCast.z) + new Vector3(offset.x, 0, offset.y); 
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
                            switch (type)
                            {
                                case Inventory.Resource.Stromg:
                                    resource.transform.parent = rockFolder.transform;
                                    break;
                                case Inventory.Resource.Zaza:
                                    resource.transform.parent = zazaFolder.transform;
                                    break;
                                case Inventory.Resource.Womp:
                                    resource.transform.parent = treeFolder.transform;
                                    break;
                            }
                            resource.transform.Rotate(Vector3.up, Random.Range(0, 180));
                            int health = Random.Range(0, 4) + 2;
                            if(type != Inventory.Resource.Zaza)
                            resource.GetComponent<InteractableResource>().health = health;
                            float scaleFactor = 1 + ((health-3) * 0.2f);
                            resource.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
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


/*#if UNITY_EDITOR
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
*/
}
