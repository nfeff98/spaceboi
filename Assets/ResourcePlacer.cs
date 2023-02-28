using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ResourcePlacer : MonoBehaviour
{
    public RenderTexture resourceMap;
    public RenderTexture distroMap;
    public Material resourceMat;
    public Material distroMat;

    private Texture2D resourceTex;
    private Texture2D distroTex;
    private int width = 1024;
    private int height = 1024;
    public Transform origin;
    public Transform resourceFolder;

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
        for (int i = 0; i < resources.Count; i++)
        {
            GameObject g = resources[i];
            DestroyImmediate(g, false);
        }
        resources.Clear();
    }

    public void PlaceNewResources()
    {
        StartCoroutine(PlaceResourceRoutine());
    }

    public IEnumerator PlaceResourceRoutine()
    {
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
        DestroyOldResources();
        ReadMap();
        for (int y = 0; y < resolution; y += step)
        {
            for (int x = 0; x < resolution; x += step)
            {

                Color val2 = distroTex.GetPixel(x, y); // could use more probability here
                if (val2.r + val2.g + val2.b > threshold)
                {
                    //read resource map and get color
                    Color val = resourceTex.GetPixel(x, y);
                    //Debug.Log(val);
                    List<GameObject> possiblePrefabs = new List<GameObject>();
                    if (val.r >= val.g && val.r >= val.b)
                    {
                        possiblePrefabs = womps;
                    }
                    else if (val.g >= val.r && val.g >= val.b)
                    {
                        possiblePrefabs = zazas;

                    }
                    else if (val.b >= val.r && val.b >= val.g)
                    {
                        possiblePrefabs = stromgs;
                    }

                    float scalar = scaleFactor * 10f;
                    Vector3 placerCast = new Vector3(x * scalar/resolution, 0, y * scalar/resolution) + origin.position;
                    Vector3 resourcePos = placerCast;
                    Ray ray = new Ray(placerCast, Vector3.down);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 20f))
                    {
                        if (hit.collider.gameObject.tag == "Navigation")
                        {
                            int index = Random.Range(0, possiblePrefabs.Count - 1);
                            resourcePos.y = hit.point.y;
                            GameObject resource = Instantiate(possiblePrefabs[index]);
                            resources.Add(resource);
                            resource.transform.position = resourcePos;
                            resource.transform.parent = resourceFolder;
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


    public void ReadMap()
    {
        width = 1024;
        height = 1024;
        resourceTex = new Texture2D(width, height, TextureFormat.RGB24, false);
        distroTex = new Texture2D(width, height, TextureFormat.RGB24, false);
        Rect reader = new Rect(0, 0, width, height);
        RenderTexture.active = resourceMap;
        resourceTex.ReadPixels(reader, 0, 0); 
        resourceTex.Apply();
        RenderTexture.active = distroMap;
        distroTex.ReadPixels(reader, 0, 0);
        distroTex.Apply();
    }


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
        }
    }
}
