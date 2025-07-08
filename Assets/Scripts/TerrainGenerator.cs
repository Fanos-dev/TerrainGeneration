using System.Collections.Generic;
using UnityEngine;

//Make sure mesh filter exists when adding script to game object 
[RequireComponent(typeof(MeshFilter))]
public class TerrainGenerator : MonoBehaviour
{
    //Terrain size
    public int sizeX = 1;
    public int sizeZ = 1;
    
    //Terrain components
    private List<Vector3> vertices;
    private List<int> triangles;

    private Mesh terrain;
    
    void Start()
    {
        terrain = new Mesh
        {
            name = "TerrainMesh"
        };

        GetComponent<MeshFilter>().mesh = terrain;

        CreateTerrain();

        UpdateTerrain();
    }

    void CreateTerrain()
    {
        vertices = new List<Vector3>((sizeX + 1) * (sizeZ + 1));
        triangles = new List<int>(sizeX * sizeZ * 6);
        
        //Create vertices
        for (int i = 0; i <= sizeX; i++)
        {
            for (int j = 0; j <= sizeZ; j++)
            {
                vertices.Add(new Vector3(i, 0, j));
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
    }

    void UpdateTerrain()
    {
        terrain.Clear();

        terrain.vertices = vertices.ToArray();
        terrain.triangles = triangles.ToArray();
        
        terrain.RecalculateNormals();
    }

    private void OnDrawGizmos()
    {
        if (vertices == null)
        {
            return;
        }
        
        foreach (var t in vertices)
        {
            Gizmos.DrawSphere(t, .1f);
        }
    }
}
