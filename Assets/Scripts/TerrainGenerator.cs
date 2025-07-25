using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

//Make sure mesh filter exists when adding script to game object 
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainGenerator : MonoBehaviour
{
    //Terrain size
    public int sizeX = 1;
    public int sizeZ = 1;

    //Control detail of terrain
    public float amplitude = 5f;
    public float frequency = 0.1f;
    
    //Terrain components
    private List<Vector3> vertices;
    private List<int> triangles;

    private Mesh terrain;

    private bool init;
    
    void Start()
    {
        GenerateTerrain();
    }

    public void GenerateTerrain()
    {
        if (!init)
        {
            terrain = new Mesh
            {
                name = "TerrainMesh",
                indexFormat = IndexFormat.UInt32
            };

            GetComponent<MeshFilter>().mesh = terrain;

            init = true;
        }
        
        vertices = new List<Vector3>((sizeX + 1) * (sizeZ + 1));
        triangles = new List<int>(sizeX * sizeZ * 6);
        
        //Create vertices
        for (int x = 0; x <= sizeX; x++)
        {
            for (int z = 0; z <= sizeZ; z++)
            {
                //Write custom perlin noise
                float y = (Mathf.PerlinNoise(x * 1.3f * frequency, z * 1.3f * frequency ) * 1f * amplitude);
                vertices.Add(new Vector3(x, y, z));
            }
        }

        //Create triangles
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeZ; j++)
            {
                //Vertices location
                int bottomLeft = i * (sizeZ + 1) + j;
                int bottomRight = (i + 1) * (sizeZ + 1) + j;
                int topLeft = bottomLeft + 1;
                int topRight = bottomRight + 1;
                    
                //Bottom left triangle 
                triangles.Add(bottomRight);
                triangles.Add(bottomLeft);
                triangles.Add(topLeft);
                
                //Top right triangle
                triangles.Add(topLeft);
                triangles.Add(topRight);
                triangles.Add(bottomRight);
            }
        }
        
        //Setup terrain
        terrain.Clear();

        terrain.vertices = vertices.ToArray();
        terrain.triangles = triangles.ToArray();
        
        terrain.RecalculateNormals();
    }

    // private void OnDrawGizmos()
    // {
    //     if (vertices == null)
    //     {
    //         return;
    //     }
    //     
    //     foreach (var t in vertices)
    //     {
    //         Gizmos.DrawSphere(t, .1f);
    //     }
    // }
}
