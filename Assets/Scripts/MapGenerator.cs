using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class MapGenerator : MonoBehaviour
{
    private bool debug = false;
    public RenderTexture heightMap;
    private Texture2D debugMap;
    public MeshCollider mapCollider;
    public MeshFilter mapFilter;
    private Mesh newMesh;
    public Mesh sourceMesh;
    public NavMeshSurface navMesh;

    private Texture2D tex;

    private int width = 1024;
    private int height = 1024;

    public Material distroMat;
    public Material paintMat;
    public Material heightMat;
    public Material grassMat;
    public float waterLevel;
    public GameObject waterLevelPlane;

    //TODO: take terrainMat and heightMat and on generate new map
    // roll random variables for each of the parameters to gen a new level

    public float magnitude;

    private void Awake()
    {
       
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public IEnumerator DelayGenerate()
    {
        //Debug.Log("Generating new map.");
        
        //Debug.Log("Rolling new random values.");
        RollParams();
        yield return new WaitForSeconds(0.1f);
        //Debug.Log("Applying effects of climate.");
        WeatherEffects();
        GenerateCollider();
    }

    public void GenerateNewMap()
    {
        StartCoroutine(DelayGenerate());

    }

    public void WeatherEffects()
    {
        //take in climate from planet
        //tint grass/dirt
        //set water level
        //Debug.Log("Setting Water Level.");
        waterLevelPlane.transform.localPosition = new Vector3(waterLevelPlane.transform.localPosition.x, waterLevel - 0.9f, waterLevelPlane.transform.localPosition.z);

    }

    public bool RollParams()
    {
        float perlinSeed = Random.Range(0, 1000);
        float riverWidth = Random.Range(0.2f, 0.4f);
        float riverBendiness = Random.Range(-0.5f, 0.5f);
        float riverRotation = Random.Range(-0.2f, 0.2f);
        float riverPosition = Random.Range(-0.6f, 0.6f);

        //both maps
        paintMat.SetFloat("_PerlinSeed", perlinSeed);
        paintMat.SetFloat("_RiverWidth", riverWidth);
        paintMat.SetFloat("_RiverBendiness", riverBendiness);
        paintMat.SetFloat("_RiverRotation", riverRotation);
        paintMat.SetFloat("_RiverPosition", riverPosition);

        heightMat.SetFloat("_PerlinSeed", perlinSeed);
        heightMat.SetFloat("_RiverWidth", riverWidth);
        heightMat.SetFloat("_RiverBendiness", riverBendiness);
        heightMat.SetFloat("_RiverRotation", riverRotation);
        heightMat.SetFloat("_RiverPosition", riverPosition);

        distroMat.SetFloat("_PerlinSeed", perlinSeed);
        distroMat.SetFloat("_RiverWidth", riverWidth);
        distroMat.SetFloat("_RiverBendiness", riverBendiness);
        distroMat.SetFloat("_RiverRotation", riverRotation);
        distroMat.SetFloat("_RiverPosition", riverPosition);

        grassMat.SetFloat("_PerlinSeed", perlinSeed);
        grassMat.SetFloat("_RiverWidth", riverWidth);
        grassMat.SetFloat("_RiverBendiness", riverBendiness);
        grassMat.SetFloat("_RiverRotation", riverRotation);
        grassMat.SetFloat("_RiverPosition", riverPosition);

        width = heightMap.width;
        height = heightMap.height;

        ReadMap();
        return true;

    }

    public void ReadMap()
    {
        tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        Rect reader = new Rect(0, 0, width, height);
        if (!debug)
        {
            RenderTexture.active = heightMap;
            tex.ReadPixels(reader, 0, 0);
        }
        else
        {
            width = debugMap.width;
            height = debugMap.height;
            tex = debugMap;
        }
        tex.Apply();
    }

    public void GenerateCollider()
    {
        newMesh = MeshCopy(sourceMesh);
        //Debug.Log("Generating new MeshCollider points.");
        ReadMap();
        List<Vector3> verts = new List<Vector3>();
        //newMesh.GetVertices(verts);
        newMesh.GetVertices(verts);

        tex.Apply();
        
        for (int i = 0; i < verts.Count; i++)
        {
            Vector3 vert = verts[i];

            
            Vector2 uv = new Vector2(vert.x, vert.z) / 10 + new Vector2(0.5f, 0.5f);
            Vector2 pix = new Vector2(uv.x * width, uv.y * height);
            Color val = tex.GetPixel((int)pix.x, (int)pix.y);
            if (debug)
                Debug.Log("Color at " + pix + " = " + val);
            float sum = val.g + val.r + val.b;
            
            vert.y = sum * magnitude;
            vert.y *= magnitude;
            
            verts[i] = vert;
        }
        

        newMesh.vertices = verts.ToArray();
        newMesh.RecalculateBounds();
        mapCollider.sharedMesh = newMesh;
        mapFilter.sharedMesh = newMesh;
        navMesh.RemoveData();
        navMesh.BuildNavMesh();
    }


    Mesh MeshCopy(Mesh sourceMesh)
    {
        Mesh mesh = new Mesh()
        {
            vertices = sourceMesh.vertices,
            triangles = sourceMesh.triangles,
            normals = sourceMesh.normals,
            tangents = sourceMesh.tangents,
            bounds = sourceMesh.bounds,
            uv = sourceMesh.uv
        };
        
        return mesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MapGenerator))]
    public class MapGeneratorEditor : Editor
    {


        public override void OnInspectorGUI()
        {

            MapGenerator mapGen = (MapGenerator)target;
            base.OnInspectorGUI();
            if (GUILayout.Button("RerollParams"))
            {
                mapGen.RollParams();
            }

            if (GUILayout.Button("Generate Mesh"))
            {
                mapGen.GenerateCollider();
            }
        }
    }
#endif
}


