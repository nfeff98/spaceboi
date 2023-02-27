using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapGenerator : MonoBehaviour
{
    public bool debug;
    public RenderTexture heightMap;
    public Texture2D debugMap;
    public RenderTexture resourceMap;
    public MeshCollider mapCollider;
    public MeshFilter mapFilter;
    public Mesh newMesh;
    public Mesh sourceMesh;

    public Texture2D tex;

    public int width;
    public int height;
    

    //TODO: take terrainMat and heightMat and on generate new map
    // roll random variables for each of the parameters to gen a new level

    public float magnitude;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void GenerateNewMap()
    {
        Debug.Log("Generating new map.");
        newMesh = MeshCopy(sourceMesh);
        width = heightMap.width;
        height = heightMap.height;
        Debug.Log("Generating new MeshCollider points");
        GenerateCollider();

    }

    public void GenerateCollider()
    {
        List<Vector3> verts = new List<Vector3>();
        //newMesh.GetVertices(verts);
        newMesh.GetVertices(verts);
        tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        Rect reader = new Rect(0, 0, width, height);
        if (!debug)
        {
            RenderTexture.active = heightMap;
            tex.ReadPixels(reader, 0, 0);
        } else
        {
            width = debugMap.width;
            height = debugMap.height;
            tex = debugMap;
        }
        tex.Apply();

        Debug.Log(verts[0]);
        
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
        

        Debug.Log(verts[0]);
        newMesh.vertices = verts.ToArray();
        newMesh.RecalculateBounds();
        mapCollider.sharedMesh = newMesh;
        mapFilter.sharedMesh = newMesh;
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

    [CustomEditor(typeof(MapGenerator))]
    public class MapGeneratorEditor : Editor
    {


        public override void OnInspectorGUI()
        {

            MapGenerator mapGen = (MapGenerator)target;
            base.OnInspectorGUI();
            if (GUILayout.Button("Generate Map"))
            {
                mapGen.GenerateNewMap();
            }
        }
    }
}


